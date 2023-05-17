using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Application.Email;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebLearning.Application.BookingCalender.Services
{
    public interface IAppointmentService
    { 
        Task<Result> PutAppointment(int id, AppointmentSlotUpdate update, string email);
        Task<Result> CreateSlotInSixMonth(AppointmentSlotRange range);

        Task<Result> CreateAppointment(int id, AppointmentSlotRequest slotRequest, string name);
        Task<IEnumerable<AppointmentSlotDto>> GetAppointments([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int? doctor);
        Task<IEnumerable<AppointmentSlotDto>> AppointmentsFree([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string patient);

        Task<Result> AdminCreateNewSingleAppointment(AppointmentSlotRange range);

        Task<Result> AdminDeleteMultiAppointment(ClearRange range);
        Task<Result> DeleteSingleAppointment(int id);
        Task<List<Result>> CreateAppointmentSlotAdvance(CreateAppointmentSlotAdvance createAppointmentSlotAdvance);

        Task<Result> ConfirmBookingAccepted(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email);
        Task<Result> ConfirmBookingRejected(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email);
        Task<Result> MailReplyAccepted(Guid fromId, Guid toId);
        Task<Result> MailReplyRejected(Guid fromId, Guid toId);

    }
    public class AppointmentService : IAppointmentService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AppointmentService(WebLearningContext context, IMapper mapper, IConfiguration configuration, IEmailSender emailSender)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public async Task<Result> AdminCreateNewSingleAppointment(AppointmentSlotRange range)
        {
            Result rs = new();
            var room = await _context.Rooms.FindAsync(range.Resource);

            if (room == null)
            {
                rs.message = "Không tìm thấy phòng";
                return rs;
            }

            var slots = Timeline.GenerateSlots(range.Start, range.End, range.Scale);
            slots.ForEach(slot =>
            {
                slot.Room = room;
                _context.Appointments.Add(slot);
            });

            await _context.SaveChangesAsync();

            rs.message = "Thêm thành công! ";

            return rs;
        }

        public async Task<Result> AdminDeleteMultiAppointment(ClearRange range)
        {
            Result result = new();
            var start = range.Start;
            var end = range.End;

            _context.Appointments.RemoveRange(_context.Appointments.Where(e => e.Status == "free" && !((e.End <= start) || (e.Start >= end))));
            await _context.SaveChangesAsync();

            result.message = "Xóa thành công!";

            return result;
        }

        public async Task<IEnumerable<AppointmentSlotDto>> AppointmentsFree([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string patient)
        {
            var ap = await _context.Appointments.Where(e => (e.Status == "free" || (e.Status != "free" && e.PatientId == patient)) && !((e.End <= start) || (e.Start >= end))).Include(e => e.Room).ToListAsync();

            var apDto = _mapper.Map<List<AppointmentSlotDto>>(ap);

            return apDto;
        }

        public async Task<Result> ConfirmBookingAccepted(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId,string email)
        {
            Result result = new();

           var historyPrevious = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(fromId));

            var historyNow = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(toId));

            if(historyNow == null || historyPrevious == null) { result.message = "NotFound"; return result; }

            var room = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == historyNow.RoomId);

            var admin = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Equals(email));

            var infouser = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Contains(historyNow.Email));

            var userAccountDto = _mapper.Map<AccountDto>(infouser);

            var adminAccountDto = _mapper.Map<AccountDto>(admin);

            var roomDto = _mapper.Map<RoomDto>(room);

            var hsNowDto = _mapper.Map<HistoryAddSlotDto>(historyNow);

            var hsPreDto = _mapper.Map<HistoryAddSlotDto>(historyPrevious);

            if (updateHistoryAddSlotDto.Status == 1)
            {
                using var transaction = _context.Database.BeginTransaction();

                _context.HistoryAddSlots.Remove(historyPrevious);

                await _context.SaveChangesAsync();

                updateHistoryAddSlotDto.DescStatus= "confirmed";

                _context.HistoryAddSlots.Update(_mapper.Map(updateHistoryAddSlotDto, historyNow));

                await _context.SaveChangesAsync();

                var slot = await _context.Appointments.Where(x => x.CodeId.Equals(toId)).ToListAsync();

                slot.ForEach(slot =>
                {
                    slot.Status = "confirmed";
                    _context.Appointments.Update(slot);
                });
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


            }
            else if (updateHistoryAddSlotDto.Status == 3)
            {
                using var transaction = _context.Database.BeginTransaction();

                updateHistoryAddSlotDto.DescStatus = "confirmed";

                _context.HistoryAddSlots.Update(_mapper.Map(updateHistoryAddSlotDto, historyNow));

                await _context.SaveChangesAsync();


                var slot = await _context.Appointments.Where(x => x.CodeId.Equals(toId)).ToListAsync();

                slot.ForEach(slot =>
                {
                    slot.Status = "confirmed";
                    _context.Appointments.Update(slot);
                });
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

            }
            else if (updateHistoryAddSlotDto.Status == 0)
            {
                using var transaction = _context.Database.BeginTransaction();

                _context.HistoryAddSlots.Remove(historyNow);

                await _context.SaveChangesAsync();


                var slot = await _context.Appointments.Where(x => x.CodeId.Equals(toId)).ToListAsync();

                slot.ForEach(slot =>
                {
                    slot.Status = "free";
                    slot.Note = "";
                    slot.Description = "";
                    slot.Email = "";
                    slot.PatientName = "";
                    slot.PatientId = "";
                    slot.CodeId = Guid.Empty;
                    _context.Appointments.Update(slot);
                });
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            var message = Extension.ConfirmSlotMesseageAccepted(email, userAccountDto, room.Name, hsNowDto.Description, hsNowDto.Note, hsNowDto.Start, hsNowDto.End);

            _emailSender.ReplyEmail(message, email, adminAccountDto.accountDetailDto.FullName, userAccountDto.Email);

            result.message = "Success";

            return result;
        }

        public async Task<Result> ConfirmBookingRejected(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email)
        {
            Result result = new();

            var historyNow = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(toId));

            if(historyNow != null)
            {
                var room = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == historyNow.RoomId);

                var admin = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Equals(email));

                var infouser = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Contains(historyNow.Email));

                var userAccountDto = _mapper.Map<AccountDto>(infouser);

                var adminAccountDto = _mapper.Map<AccountDto>(admin);

                var roomDto = _mapper.Map<RoomDto>(room);

                var hsNowDto = _mapper.Map<HistoryAddSlotDto>(historyNow);

                using var transaction = _context.Database.BeginTransaction();

                _context.HistoryAddSlots.Remove(historyNow);

                await _context.SaveChangesAsync();


                var slot = await _context.Appointments.Where(x => x.CodeId.Equals(toId)).ToListAsync();

                slot.ForEach(slot =>
                {
                    slot.Status = "free";
                    slot.Note = "";
                    slot.Description = "";
                    slot.Email = "";
                    slot.PatientName = "";
                    slot.PatientId = "";
                    slot.CodeId = Guid.Empty;
                    _context.Appointments.Update(slot);
                });
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var message = Extension.ConfirmSlotMesseageRejected(email, userAccountDto, room.Name, hsNowDto.Description, hsNowDto.Note, hsNowDto.Start, hsNowDto.End);

                _emailSender.ReplyEmail(message, email, adminAccountDto.accountDetailDto.FullName, userAccountDto.Email);

                result.message = "Success";
            }
            else
            {
                result.message = "NotFound";
            }



            return result;
        }

        public async Task<Result> CreateAppointment(int id, AppointmentSlotRequest slotRequest, string name)
        {
            Result result = new();

            var appointmentSlot = await _context.Appointments.Include(x => x.Room).FirstOrDefaultAsync(x => x.Id == id);

            if (appointmentSlot == null)
            {
                result.message = $"Không tìm thấy phòng {id}";
                return result;
            }

            var accountDb = await _context.Accounts.Include(x => x.AccountDetail).FirstOrDefaultAsync(a => a.Email.Contains(name));

            var apDto = _mapper.Map<AppointmentSlotDto>(appointmentSlot);

            var account = _mapper.Map<AccountDto>(accountDb);

            appointmentSlot.PatientName = " - " + "Người đặt: " + account.Email + "\n" + " - " + "Nội dung: " + slotRequest.Description;

            appointmentSlot.PatientId = slotRequest.Patient;

            appointmentSlot.Email = name;

            appointmentSlot.Note = slotRequest.Note;

            appointmentSlot.Description = slotRequest.Description;

            if (slotRequest.Description == "")
            {
                appointmentSlot.Note = "Không";
            }
            appointmentSlot.Status = "waiting";

            appointmentSlot.CodeId = Guid.NewGuid();


            _context.Appointments.Update(appointmentSlot);

            await _context.SaveChangesAsync();

            CreateHistoryAddSlotDto createHistoryAddSlotDto = new()
            {
                Email = name,
                Note = slotRequest.Note,
                Description = slotRequest.Description,
                Start = appointmentSlot.Start,
                End = appointmentSlot.End,
                Status = "waiting",
                CodeId = appointmentSlot.CodeId,
                OldCodeId = appointmentSlot.CodeId,
                TypedSubmit = "Create",
                RoomId = appointmentSlot.Room.Id,
            };
            HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(createHistoryAddSlotDto);

            _context.HistoryAddSlots.Add(historyAddSlot);

            await _context.SaveChangesAsync();

            var baseAddress = _configuration.GetValue<string>("BaseAddress"); 
            
            var emailAdmin = _configuration.GetValue<string>("EmailConfiguration:To");


            var message = Extension.CreateMessageSingleSlot(baseAddress, emailAdmin, account, appointmentSlot.DoctorName, slotRequest.Description, slotRequest.Note, appointmentSlot, appointmentSlot.CodeId, appointmentSlot.Start, appointmentSlot.End);

            _emailSender.SendEmail(message, account.Email, account.accountDetailDto.FullName);

            result.message = "Đặt lịch thành công";

            return result;
        }

        public async  Task<List<Result>> CreateAppointmentSlotAdvance(CreateAppointmentSlotAdvance createAppointmentSlotAdvance)
        {
            List<Result> result = new();
            var slot = await _context.Appointments.Where(x => x.Room.Id == createAppointmentSlotAdvance.Room).ToListAsync();

            if(slot.Count == 0)
            {
                result.Add(new Result
                {
                    message = "Room Not Found"
                });

                return result;
            }
            var room = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == createAppointmentSlotAdvance.Room);

            var account = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email == createAppointmentSlotAdvance.Email);

            var roomDto = _mapper.Map<RoomDto>(room);

            var accountDto = _mapper.Map<AccountDto>(account);

            var BaseAddressAddMulti = _configuration.GetValue<string>("BaseAddress");

            var emailAdmin = _configuration.GetValue<string>("EmailConfiguration:To");

            if (createAppointmentSlotAdvance.TypedSubmit == 1)
            {
                var dateForWeek = Extension.DaysWeakly(createAppointmentSlotAdvance.Start, createAppointmentSlotAdvance.End, createAppointmentSlotAdvance.TypedSubmit);
                Guid code = Guid.NewGuid();

                foreach (var date in dateForWeek)
                {
                    var listSlot = Extension.SortList(date.Start, date.End, slot);
                    var exist = listSlot.Where(x => x.Status != "free").ToList();
                    if (exist != null && exist.Count > 0)
                    {
                        foreach (var nf in exist)
                        {
                            result.Add(new Result
                            {
                                message = nf.Start + " - " + nf.End + " đã có người đặt!"
                            });

                        }
                        return result;

                    }
                    else
                    {
                        listSlot.ForEach(item =>
                        {
                            item.PatientName = " - " + "Người đặt: " + createAppointmentSlotAdvance.Email + "\n" + " - " + "Nội dung: " + createAppointmentSlotAdvance.Description;


                            item.PatientId = "d2cdd687-3f3d-d71b-258a-06906cb82f44";

                            item.Status = "waiting";

                            item.Email = createAppointmentSlotAdvance.Email;

                            item.Description = createAppointmentSlotAdvance.Description;

                            item.Note = createAppointmentSlotAdvance.Note;

                            item.CodeId = code;

                            _context.Appointments.Update(item);
                        });

                        await _context.SaveChangesAsync();
                    }

                }
                var historySubmit = new CreateHistoryAddSlotDto()
                {
                    Email = createAppointmentSlotAdvance.Email,
                    Note = createAppointmentSlotAdvance.Note,
                    Description = createAppointmentSlotAdvance.Description,
                    Start = createAppointmentSlotAdvance.Start,
                    End = createAppointmentSlotAdvance.End,
                    Status = "waiting",
                    CodeId = code,
                    RoomId = createAppointmentSlotAdvance.Room,
                };

                HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(historySubmit);

                _context.HistoryAddSlots.Add(historyAddSlot);
                await _context.SaveChangesAsync();

                var message = Extension.CreateSlotMesseageWeeklyInMonth(BaseAddressAddMulti, emailAdmin, accountDto, roomDto,
                     createAppointmentSlotAdvance.Description,  createAppointmentSlotAdvance.Note, code,createAppointmentSlotAdvance.Start,createAppointmentSlotAdvance.End);

                _emailSender.SendEmail(message, account.Email, account.AccountDetail.FullName);

                result.Add(new Result
                {
                    message = "Success"
                });
                return result;
            }
            if (createAppointmentSlotAdvance.TypedSubmit == 2)
            {
                Guid code = Guid.NewGuid();

                var dateForWeek = Extension.DaysWeakly(createAppointmentSlotAdvance.Start, createAppointmentSlotAdvance.End, createAppointmentSlotAdvance.TypedSubmit);

                foreach (var date in dateForWeek)
                {
                    var listSlot = Extension.SortList(date.Start, date.End, slot);
                    var exist = listSlot.Where(x => x.Status != "free").ToList();

                    if (exist != null && exist.Count > 0)
                    {
                        foreach (var nf in exist)
                        {
                            result.Add(new Result
                            {
                                message = nf.Start + " - " + nf.End + " đã có người đặt!"
                            });

                        }
                        return result;

                    }
                    else
                    {
                        listSlot.ForEach(item =>
                        {
                            item.PatientName = " - " + "Người đặt: " + createAppointmentSlotAdvance.Email + "\n" + " - " + "Nội dung: " + createAppointmentSlotAdvance.Description;


                            item.PatientId = "d2cdd687-3f3d-d71b-258a-06906cb82f44";

                            item.Status = "waiting";

                            item.Email = createAppointmentSlotAdvance.Email;

                            item.Description = createAppointmentSlotAdvance.Description;

                            item.Note = createAppointmentSlotAdvance.Note;

                            item.CodeId = code;

                            _context.Appointments.Update(item);
                        });

                        await _context.SaveChangesAsync();

                    }

                }

                var historySubmit = new CreateHistoryAddSlotDto()
                {
                    Email = createAppointmentSlotAdvance.Email,
                    Note = createAppointmentSlotAdvance.Note,
                    Description = createAppointmentSlotAdvance.Description,
                    Start = createAppointmentSlotAdvance.Start,
                    End = createAppointmentSlotAdvance.End,
                    Status = "waiting",
                    CodeId = code,
                    RoomId = createAppointmentSlotAdvance.Room,
                };
                HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(historySubmit);

                _context.HistoryAddSlots.Add(historyAddSlot);

                await _context.SaveChangesAsync();

                var message = Extension.CreateSlotMesseageWeeklyInMultiMonth(BaseAddressAddMulti,emailAdmin, accountDto, roomDto,
                            createAppointmentSlotAdvance.Description, createAppointmentSlotAdvance.Note,code,createAppointmentSlotAdvance.Start,
                            createAppointmentSlotAdvance.End);

                _emailSender.SendEmail(message, account.Email, account.AccountDetail.FullName);

                result.Add(new Result
                {
                    message = "Success"
                });
                return result;

            }
            else
            {

                var final = Extension.SortList(createAppointmentSlotAdvance.Start, createAppointmentSlotAdvance.End, slot);

                //12 trưa là 12:00 PM

                var exist = final.Where(x => x.Status != "free").ToList();
                Guid code = Guid.NewGuid();

                if (exist != null && exist.Count > 0)
                {
                    foreach (var nf in exist)
                    {
                        result.Add(new Result
                        {
                            message = nf.Start + " - " + nf.End + " đã có người đặt!"
                        });

                    }
                    return result;

                }
                else
                {
                    final.ForEach(item =>
                    {
                        item.PatientName = " - " + "Người đặt: " + createAppointmentSlotAdvance.Email + "\n" + " - " + "Nội dung: " + createAppointmentSlotAdvance.Description;

                        item.PatientId = "d2cdd687-3f3d-d71b-258a-06906cb82f44";

                        item.Status = "waiting";

                        item.Email = createAppointmentSlotAdvance.Email;
                        item.Description = createAppointmentSlotAdvance.Description;

                        item.Note = createAppointmentSlotAdvance.Note;
                        item.CodeId = code;
                        _context.Appointments.Update(item);
                    });

                    await _context.SaveChangesAsync();

                    var historySubmit = new CreateHistoryAddSlotDto()
                    {
                        Email = createAppointmentSlotAdvance.Email,
                        Note = createAppointmentSlotAdvance.Note,
                        Description = createAppointmentSlotAdvance.Description,
                        Start = createAppointmentSlotAdvance.Start,
                        End = createAppointmentSlotAdvance.End,
                        Status = "waiting",
                        CodeId = code,
                        RoomId = createAppointmentSlotAdvance.Room,
                    };
                    HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(historySubmit);

                    _context.HistoryAddSlots.Add(historyAddSlot);
                    await _context.SaveChangesAsync();
                }

                if (final.Count > 0)
                {
                    var message = Extension.CreateMessageAdvance(BaseAddressAddMulti,emailAdmin, accountDto, roomDto.Name,
                        createAppointmentSlotAdvance.Description,createAppointmentSlotAdvance.Note,code,createAppointmentSlotAdvance.Start,createAppointmentSlotAdvance.End);

                    _emailSender.SendEmail(message, account.Email, account.AccountDetail.FullName);

                    result.Add(new Result
                    {
                        message = "Success"
                    });
                    return result;

                }



            }
            result.Add(new Result
            {
                message = "Success"
            }) ;

            return result;
        }

        public async Task<Result> CreateSlotInSixMonth(AppointmentSlotRange range)
        {
            Result rs = new();
            try
            {
                var exist = await _context.Appointments.Where(x => x.Status == "free").OrderByDescending(x => x.End).ToListAsync();
                foreach (var i in await _context.Rooms.ToListAsync())
                {
                    var busy = await _context.Appointments.Where(x => x.Status != "free" && x.Room.Id == i.Id).OrderByDescending(x => x.End).ToListAsync();

                    range.Resource = i.Id;
                    var room = await _context.Rooms.FindAsync(range.Resource);

                    if (room == null)
                    {
                        rs.message = "Không tìm thấy phòng";

                        return rs;
                    }
                    List<AppointmentSlot> listSlotBeforeDateStart = new();
                    if (busy.Count > 0)
                    {
                        listSlotBeforeDateStart = await _context.Appointments.Where(x => x.Start <= busy[0].End && x.Status != "free" && x.Room.Id == i.Id).OrderByDescending(x => x.End).ToListAsync();

                    }
                    var listSlotAfterDateStart = await _context.Appointments.Where(x => x.Start >= range.Start && x.Status != "free" && x.Room.Id == i.Id).OrderByDescending(x => x.End).ToListAsync();

                    var resultCheck = Extension.CheckFreeSlot(exist, busy, listSlotBeforeDateStart, listSlotAfterDateStart, range);

                    if (resultCheck.SlotFreeInBusyTimeAndGreateThanDateNow != null && resultCheck.SlotFreeInBusyTimeAndGreateThanDateNow.Count > 0)
                    {
                        foreach (var slF in resultCheck.SlotFreeInBusyTimeAndGreateThanDateNow)
                        {
                            var free = await _context.Appointments.Where(x => x.Start == slF.Start && x.End == slF.End).ToListAsync();
                            if (free.Count == 0)
                            {
                                slF.Room = room;
                                _context.Appointments.Add(slF);
                                await _context.SaveChangesAsync();

                            }
                        }
                    }


                    resultCheck.TotalSlotInMonth.ForEach(slot =>
                    {
                        slot.Room = room;
                        _context.Appointments.Add(slot);
                    });
                    await _context.SaveChangesAsync();

                    if (resultCheck.TotalSlotFreeBeforeDateNow != null)
                    {
                        var checkFreeInPrevoiusSlot = resultCheck.TotalSlotFreeBeforeDateNow.Where(x => x.Status == "free").ToList();
                        checkFreeInPrevoiusSlot.ForEach(slot =>
                        {
                            foreach (var slF in checkFreeInPrevoiusSlot)
                            {
                                if (slot.Start != slF.Start && slot.End != slF.End)
                                {
                                    slot.Room = room;
                                    _context.Appointments.Add(slot);
                                }
                            }

                        });
                        await _context.SaveChangesAsync();

                    }

                }
            }
            catch (Exception ex)
            {
                rs.message = ex.Message;
                return rs;
            }


            rs.message = "Tạo thành công!";

            return rs;
        }

        public async Task<Result> DeleteSingleAppointment(int id)
        {
            Result rs = new();
            var appointmentSlot = await _context.Appointments.FindAsync(id);

            if (appointmentSlot == null)
            {
                rs.message = "Không tìm thấy chỗ trống này";
                return rs;
            }

            _context.Appointments.Remove(appointmentSlot);

            await _context.SaveChangesAsync();

            rs.message = "Xóa thành công!";

            return rs;
        }

        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointments([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int? doctor)
        {
            if (doctor == null)
            {
                var ap = await _context.Appointments.Where(e => !((e.End <= start) || (e.Start >= end))).Include(e => e.Room).ToListAsync();

                var apDto = _mapper.Map<List<AppointmentSlotDto>>(ap);

                return apDto;
            }
            else
            {
                var ap = await _context.Appointments.Where(e => e.Room.Id == doctor && !((e.End <= start) || (e.Start >= end))).Include(e => e.Room).ToListAsync();

                var apDto = _mapper.Map<List<AppointmentSlotDto>>(ap);

                return apDto;
            }

        }

        public async Task<Result> MailReplyAccepted(Guid fromId, Guid toId)
        {
            Result result = new();

            var history = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(toId));

            if (history == null)
            {
                result.message = "NotFound";

                return result;
            }
            var room = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == history.RoomId);

            var infouser = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Contains(history.Email));
            
            var accountDto = _mapper.Map<AccountDto>(infouser);

            var roomDto = _mapper.Map<RoomDto>(room);

            var historyDto = _mapper.Map<HistoryAddSlotDto>(history);

            var emailAdmin = _configuration.GetValue<string>("EmailConfiguration:To");

            var baseAddress = _configuration.GetValue<string>("BaseAddress");


            //var body = "Nội dung cuộc họp: " + history.Description + "<br/>" +
            //            "Ghi chú: " + history.Note + "<br/>" +
            //            "Họ tên người đặt: " + infouser.Name + "<br/>" +
            //            "Email: " + infouser.Email + "<br/>" +
            //            "Bộ phận: " + infouser.Department + "<br/>" +
            //            "Phòng: " + room.Name + "<br/>" +
            //            "Thời gian bắt đầu: " + history.Start + "<br/>" +
            //            "Thời gian kết thúc: " + history.End + "<br/>" +
            //            "Trang thái: " + "<span style='background-color:#5cb85c;display: inline;padding: .3em .7em .4em;font-size: 75%;font-weight: 700;line-height: 1;color: #fff;text-align: center;white-space: nowrap;vertical-align: baseline;border-radius: .25em;'>" +
            //                        "Xác nhận" +
            //                        "</span>" + "<br/><br/>" +
            //            "<div>" +
            //                "<a style='padding: 6px 12px;background-color: #5bc0de;border-radius:30px;text-decoration: none;color:#fff;' " +
            //                "href = " + baseAddress + fromId + "/" + toId + "&trang-thai=1" + " >Duyệt</a>" + "<br/><br/>" +
            //             "</div>" + "</div></div>";

            //var message = new Message(new string[] { $"{emailAdmin}" }, "XÁC NHẬN DỜI LỊCH", $"{body}");
            var message = Extension.ConfirmMoveSlotMesseageAccepted(baseAddress, emailAdmin, accountDto, roomDto.Name,
            historyDto.Description, historyDto.Note, historyDto.Start,
            historyDto.End,fromId,toId);
            _emailSender.ReplyEmail(message, accountDto.Email, accountDto.accountDetailDto.FullName, emailAdmin);
            return result;
        }

        public Task<Result> MailReplyRejected(Guid fromId, Guid toId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> PutAppointment(int id, AppointmentSlotUpdate update, string email)
        {
            Result result = new();
            var appointmentSlot = await _context.Appointments.Include(x => x.Room).SingleOrDefaultAsync(x => x.Id == id);
            if (appointmentSlot.CodeId == Guid.Empty)
            {
                result.message = $"Mã CodeId (Id:{appointmentSlot.Id}) bị lỗi. Liên hệ IT để được xử lí!";
                return result;
            }
            var histortyAdd = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(appointmentSlot.CodeId));
            var accountDb = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Contains(appointmentSlot.Email));
            var adminDb = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Contains(email));

            var account = _mapper.Map<AccountDto>(accountDb);

            var admin = _mapper.Map<AccountDto>(adminDb);

            var apDto = _mapper.Map<AppointmentSlotDto>(appointmentSlot);


            int rs = DateTime.Compare(update.Start, update.End);
            if (rs > 0)
            {
                result.message = "Thời gian bắt đầu phải lớn hơn thòi gian kết thúc";
                return result;
            }

            if (appointmentSlot == null)
            {
                result.message = "Không tìm thấy lịch trống";
                return result;
            }

            if (update.CustomAdd > 0)
            {
                var slot = await _context.Appointments.Where(x => x.Room.Id == update.Resource).ToListAsync();

                var newRoom = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == update.Resource);

                var oldRoom = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == histortyAdd.RoomId);

                var newRoomDto = _mapper.Map<RoomDto>(newRoom);

                var oldRoomDto = _mapper.Map<RoomDto>(oldRoom);

                var checkExist = await _context.Appointments.Where(x => x.CodeId.Equals(apDto.CodeId) ).OrderByDescending(x => x.Start).ToListAsync();


                if (update.CustomAdd == 1)
                {
                    using var transaction = _context.Database.BeginTransaction();

                    try
                    {
                        var dateForWeek = Extension.DaysWeakly(update.Start, update.End, update.CustomAdd);


                        Guid codeId = Guid.NewGuid();

                        CreateHistoryAddSlotDto createHistoryAddSlotDto = new()
                        {
                            CodeId = codeId,
                            OldCodeId = appointmentSlot.CodeId,
                            TypedSubmit = "Edit",
                            Email = appointmentSlot.Email,
                            Description = appointmentSlot.Description,
                            Note = appointmentSlot.Note,
                            Status = "waiting",
                            RoomId = newRoom.Id,
                            Start = update.Start,
                            End = update.End,
                        };
                        HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(createHistoryAddSlotDto);


                        _context.HistoryAddSlots.Add(historyAddSlot);
                        await _context.SaveChangesAsync();

                        foreach (var date in dateForWeek)
                        {
                            var listSlot = Extension.SortList(date.Start, date.End, slot);

                            var existConfirmed = listSlot.Where(x => x.Status != "free").ToList();

                            if (existConfirmed != null && existConfirmed.Count > 0 && existConfirmed[0].CodeId.Equals(apDto.CodeId) == false)
                            {
                                result.message = "Khoảng thời gian bạn chọn đã được đặt";
                                return result;
                            }

                            else
                            {
                                listSlot.ForEach(item =>
                                {
                                    item.PatientName = " - " + "Người đặt: " + account.Email + "\n" + " - " + "Nội dung: " + update.Description;


                                    item.PatientId = "d2cdd687-3f3d-d71b-258a-06906cb82f44";

                                    item.Status = "waiting";

                                    item.Email = account.Email;

                                    item.Description = update.Description;

                                    item.Note = update.Note;

                                    item.CodeId = codeId;

                                    _context.Appointments.Update(item);

                                });

                                await _context.SaveChangesAsync();


                            }

                            foreach (var exist in checkExist.ToList())
                            {

                                if (exist.Start >= date.Start && exist.End == date.End && exist.Resource == update.Resource)
                                {
                                    checkExist.Remove(exist);
                                }
                            }


                        }




                        var baseAddress = _configuration.GetValue<string>("BaseAddressChangeTime");

                        var message = Extension.MoveSlotMesseageWeeklyInMonth(baseAddress, email, account, oldRoomDto, newRoomDto, update.Description, update.Note, appointmentSlot.CodeId, codeId, histortyAdd.Start, histortyAdd.End, historyAddSlot.Start, historyAddSlot.End);

                        _emailSender.ReplyEmail(message, email, admin.accountDetailDto.FullName, account.Email);

                        if(checkExist.Count > 0)
                        {
                            checkExist.ForEach(ce =>
                            {

                                ce.PatientName = "";
                                ce.PatientId = "";
                                ce.Status = "free";
                                ce.Email = "";
                                ce.Description = "";
                                ce.Note = "";
                                ce.CodeId = Guid.Empty;
                                _context.Appointments.Update(ce);


                            });
                        }

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();


                        result.message = "Cập nhật thành công!";
                        return result;
                    }
                    catch (Exception ex)
                    {
                        result.message = ex.Message;
                        return result;

                    }

                }
                if (update.CustomAdd == 2)
                {
                    int checkToDateAndFromDate = DateTime.Compare(update.Start, update.End);
                    if (checkToDateAndFromDate >= 0 || update.Start.TimeOfDay == update.End.TimeOfDay)
                    {
                        result.message = "Thời gian không hợp lệ!";
                        return result;
                    }

                    var dateForWeek = Extension.DaysWeakly(update.Start, update.End, update.CustomAdd);
                    using var transaction = _context.Database.BeginTransaction();

                    try
                    {
                        Guid codeId = Guid.NewGuid();

                        CreateHistoryAddSlotDto createHistoryAddSlotDto = new()
                        {
                            CodeId = codeId,
                            Email = appointmentSlot.Email,
                            Description = appointmentSlot.Description,
                            Note = appointmentSlot.Note,
                            Status = "waiting",
                            RoomId = newRoom.Id,
                            Start = update.Start,
                            End = update.End,
                            OldCodeId = appointmentSlot.CodeId,
                            TypedSubmit = "Edit",
                        };
                        HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(createHistoryAddSlotDto);

                        _context.HistoryAddSlots.Add(historyAddSlot);

                        await _context.SaveChangesAsync();

                        foreach (var date in dateForWeek)
                        {

                            var listSlot = Extension.SortList(date.Start, date.End, slot);
                            var exist = listSlot.Where(x => x.Status != "free").ToList();

                            if (exist != null && exist.Count > 0 && exist[0].CodeId.Equals(apDto.CodeId) == false)
                            {
                                result.message = "Khoảng thời gian bạn chọn đã được đặt";
                                return result;
                            }
                            else
                            {
                                listSlot.ForEach(item =>
                                {
                                    item.PatientName = " - " + "Người đặt: " + account.Email + "\n" + " - " + "Nội dung: " + update.Description;


                                    item.PatientId = "d2cdd687-3f3d-d71b-258a-06906cb82f44";

                                    item.Status = "waiting";

                                    item.Email = account.Email;

                                    item.Description = update.Description;

                                    item.Note = update.Note;

                                    item.CodeId = codeId;

                                    _context.Appointments.Update(item);

                                });

                                await _context.SaveChangesAsync();

                            }
                            foreach (var e in checkExist.ToList())
                            {

                                if (e.Start >= date.Start && e.End == date.End && e.Resource == update.Resource)
                                {
                                    checkExist.Remove(e);
                                }
                            }
                        }

                        var baseAddress = _configuration.GetValue<string>("BaseAddressChangeTime");

                        var message = Extension.MoveSlotMesseageWeeklyInMultiMonth(baseAddress, email, account, oldRoomDto, newRoomDto, update.Description, update.Note, appointmentSlot.CodeId, codeId, histortyAdd.Start, histortyAdd.End, historyAddSlot.Start, historyAddSlot.End);


                        _emailSender.ReplyEmail(message, email, admin.accountDetailDto.FullName, account.Email);

                        if(checkExist.Count > 0)
                        {
                            checkExist.ForEach(ce =>
                            {
                                ce.PatientName = "";
                                ce.PatientId = "";
                                ce.Status = "free";
                                ce.Email = "";
                                ce.Description = "";
                                ce.Note = "";
                                ce.CodeId = Guid.Empty;
                                _context.Appointments.Update(ce);


                            });
                        }

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        result.message = "Cập nhật thành công!";
                        return result;
                    }
                    catch (Exception ex)
                    {
                        result.message = ex.Message;
                        return result;
                    }
                }
                else if (update.CustomAdd == 3)
                {
                    using var transaction = _context.Database.BeginTransaction();
                    try
                    {

                        var final = Extension.SortList(update.Start, update.End, slot);

                        var exist = final.Where(x => x.Status != "free").ToList();

                        if (exist != null && exist.Count > 0 && exist[0].CodeId.Equals(appointmentSlot.CodeId) == false)
                        {
                            result.message = "Khoảng thời gian bạn chọn đã được đặt";
                            return result;
                        }
                        update.Name = appointmentSlot.PatientName;

                        Guid codeId = Guid.NewGuid();

                        CreateHistoryAddSlotDto createHistoryAddSlotDto = new()
                        {
                            CodeId = codeId,
                            Email = appointmentSlot.Email,
                            Description = appointmentSlot.Description,
                            Note = appointmentSlot.Note,
                            Status = "waiting",
                            RoomId = newRoomDto.Id,
                            Start = update.Start,
                            End = update.End,
                            OldCodeId = appointmentSlot.CodeId,
                            TypedSubmit = "Edit",
                        };
                        HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(createHistoryAddSlotDto);

                        _context.HistoryAddSlots.Add(historyAddSlot);

                        await _context.SaveChangesAsync();

                        final.ForEach(item =>
                        {
                            item.PatientName = " - " + "Người đặt: " + account.Email + "\n" + " - " + "Nội dung: " + update.Description;


                            item.PatientId = "d2cdd687-3f3d-d71b-258a-06906cb82f44";

                            item.Status = "waiting";

                            item.Email = account.Email;

                            item.Description = update.Description;

                            item.Note = update.Note;

                            item.CodeId = codeId;

                            _context.Appointments.Update(item);

                        });

                        await _context.SaveChangesAsync();



                        var baseAddress = _configuration.GetValue<string>("BaseAddressChangeTime");

                        var message = Extension.MoveSlotMesseage(baseAddress, email, account, oldRoomDto, newRoomDto, update.Description, update.Note, appointmentSlot.CodeId, codeId, histortyAdd.Start, histortyAdd.End, historyAddSlot.Start, historyAddSlot.End);

                        _emailSender.ReplyEmail(message, email, admin.accountDetailDto.FullName, account.Email);

                        checkExist.ForEach(ce =>
                        {
                            ce.PatientName = "";
                            ce.PatientId = "";
                            ce.Status = "free";
                            ce.Email = "";
                            ce.Description = "";
                            ce.Note = "";
                            ce.CodeId = Guid.Empty;
                            _context.Appointments.Update(ce);


                        });
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();


                        result.message = "Cập nhật thành công!";
                        return result;
                    }
                    catch (Exception ex)
                    {
                        result.message = ex.Message;
                        return result;
                    }

                }
            }
            else if (update.CustomAdd == 0)
            {
                try
                {
                    using var transaction = _context.Database.BeginTransaction();

                    var checkExist = await _context.Appointments.Where(x => x.CodeId.Equals(appointmentSlot.CodeId)).OrderByDescending(x => x.Start).ToListAsync();

                    var history = await _context.HistoryAddSlots.Where(x => x.CodeId.Equals(appointmentSlot.CodeId)).OrderByDescending(x => x.Start).ToListAsync();

                    var historyOld = await _context.HistoryAddSlots.FirstOrDefaultAsync(x => x.CodeId.Equals(history[0].OldCodeId) == true && x.OldCodeId.Equals(history[0].OldCodeId) == true && x.TypedSubmit != "Create");

                    if(historyOld != null)
                    {
                        _context.HistoryAddSlots.Remove(historyOld);

                        await _context.SaveChangesAsync();
                    }


                    checkExist.ForEach(ce =>
                    {
                        ce.Status = update.Status;

                        _context.Appointments.Update(ce);
                    });
                    await _context.SaveChangesAsync();

                    history.ForEach(ht =>
                    {
                        ht.Status = update.Status;

                        _context.HistoryAddSlots.Update(ht);
                    });


                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    if (update.Status == "confirmed")
                    {
                        var message = Extension.ConfirmSlotMesseageAccepted(email, account, appointmentSlot.DoctorName, appointmentSlot.Description, appointmentSlot.Note, checkExist[checkExist.Count - 1].Start, checkExist[0].End);

                        _emailSender.ReplyEmail(message, email, admin.accountDetailDto.FullName, account.Email);
                    }

                }
                catch (Exception ex)
                {
                    result.message = ex.Message;
                    return result;
                }
            }

            result.message = "Cập nhật thành công!";
            return result;
        }
    }
}
