using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Email;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;
using WebLearning.Contract.Dtos.BookingCalender.Room;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

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
        Task<Result> DeleteAppointmentBooked(Guid codeId);

        Task<AppointmentSlotDto> GetAppointmentSlot(int id);
        Task<List<Result>> CreateAppointmentSlotAdvance(CreateAppointmentSlotAdvance createAppointmentSlotAdvance);

        Task<Result> ConfirmBookingAccepted(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email);
        Task<Result> ConfirmBookingRejected(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email);
        Task<Result> MailReplyAccepted(Guid fromId, Guid toId);
        Task<Result> MailReplyRejected(Guid fromId, Guid toId);
        Task<IEnumerable<HistoryAddSlotExport>> ExportHistoryAllSlotDtos(DateTime fromDate, DateTime toDate, bool confirmed, int room, string email);

    }
    public class AppointmentService : IAppointmentService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IRoomService _roomService;

        public AppointmentService(WebLearningContext context, IMapper mapper, IConfiguration configuration, IEmailSender emailSender, IRoomService roomService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _emailSender = emailSender;
            _roomService = roomService;
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
            var ap = await _context.Appointments.Where(e => (e.Status == "free" || (e.Status != "free" && e.PatientId == patient)) && !((e.End <= start) || (e.Start >= end))).Include(e => e.Room).AsNoTracking().ToListAsync();

            var apDto = _mapper.Map<List<AppointmentSlotDto>>(ap);

            return apDto;
        }

        public async Task<Result> ConfirmBookingAccepted(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email)
        {
            Result result = new();

            var historyPrevious = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(fromId));

            var historyNow = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(toId));

            if (historyNow == null || historyPrevious == null || historyNow.SendMail == true) { result.message = "NotFound"; return result; }

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

                updateHistoryAddSlotDto.SendMail = true;

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
            else if (updateHistoryAddSlotDto.Status == 3)
            {
                using var transaction = _context.Database.BeginTransaction();

                updateHistoryAddSlotDto.SendMail = true;
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
            var message = Extension.ConfirmSlotMesseageAccepted(email, userAccountDto, room.Name, hsNowDto.Description, hsNowDto.Note, hsNowDto.Start, hsNowDto.End, hsNowDto.Title);

            _emailSender.ReplyEmail(message, email, adminAccountDto.accountDetailDto.FullName, userAccountDto.Email);

            result.message = "Success";

            return result;
        }

        public async Task<Result> ConfirmBookingRejected(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email)
        {
            Result result = new();
            var historyNow = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(toId));
            var historyPrevious = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(fromId));

            if (historyNow != null && historyNow.Status != "confirmed" && historyNow.SendMail == false)
            {
                var room = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == historyNow.RoomId);

                var admin = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Equals(email));

                var infouser = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Contains(historyNow.Email));

                var userAccountDto = _mapper.Map<AccountDto>(infouser);

                var adminAccountDto = _mapper.Map<AccountDto>(admin);

                var roomDto = _mapper.Map<RoomDto>(room);

                var hsNowDto = _mapper.Map<HistoryAddSlotDto>(historyNow);

                var historyPreviousDto = _mapper.Map<HistoryAddSlotDto>(historyPrevious);

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

                var message = Extension.ConfirmSlotMesseageRejected(email, userAccountDto, room.Name, hsNowDto.Description, hsNowDto.Note, hsNowDto.Start, hsNowDto.End, hsNowDto.Title);

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

            if (accountDb == null) { result.message = "Không tìm thấy tài khoản.\nVui lòng đăng nhập trước khi đặt lịch!"; return result; }

            var apDto = _mapper.Map<AppointmentSlotDto>(appointmentSlot);

            var account = _mapper.Map<AccountDto>(accountDb);

            appointmentSlot.PatientName = slotRequest.Name;

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
            appointmentSlot.PatientName = slotRequest.Name;

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
                Editor = name,
                Title = appointmentSlot.PatientName,
                SendMail = false,
            };
            HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(createHistoryAddSlotDto);

            _context.HistoryAddSlots.Add(historyAddSlot);

            await _context.SaveChangesAsync();

            var baseAddress = _configuration.GetValue<string>("BaseAddress");

            var emailAdmin = _configuration.GetValue<string>("EmailConfiguration:To");


            var message = Extension.CreateMessageSingleSlot(baseAddress, emailAdmin, account, appointmentSlot.DoctorName, slotRequest.Description, slotRequest.Note, appointmentSlot, appointmentSlot.CodeId, appointmentSlot.Start, appointmentSlot.End, appointmentSlot.PatientName);

            _emailSender.SendEmail(message, account.Email, account.accountDetailDto.FullName);

            result.message = "Đặt lịch thành công";

            return result;
        }

        public async Task<List<Result>> CreateAppointmentSlotAdvance(CreateAppointmentSlotAdvance createAppointmentSlotAdvance)
        {
            List<Result> result = new();
            var slot = await _context.Appointments.Where(x => x.Room.Id == createAppointmentSlotAdvance.Room).ToListAsync();

            if (slot.Count == 0)
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
                            item.PatientName = createAppointmentSlotAdvance.Title;


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
                    TypedSubmit = "Create",
                    CodeId = code,
                    OldCodeId = code,
                    RoomId = createAppointmentSlotAdvance.Room,
                    Title = createAppointmentSlotAdvance.Title,
                    Editor = createAppointmentSlotAdvance.Email,
                    SendMail = false,
                };

                HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(historySubmit);

                _context.HistoryAddSlots.Add(historyAddSlot);
                await _context.SaveChangesAsync();

                var message = Extension.CreateSlotMesseageWeeklyInMonth(BaseAddressAddMulti, emailAdmin, accountDto, roomDto,
                     createAppointmentSlotAdvance.Description, createAppointmentSlotAdvance.Note, code, createAppointmentSlotAdvance.Start, createAppointmentSlotAdvance.End, createAppointmentSlotAdvance.Title);

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
                            item.PatientName = createAppointmentSlotAdvance.Title;


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
                    OldCodeId = code,
                    TypedSubmit = "Create",
                    RoomId = createAppointmentSlotAdvance.Room,
                    Title = createAppointmentSlotAdvance.Title,
                    Editor = createAppointmentSlotAdvance.Email,
                    SendMail = false,
                };
                HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(historySubmit);

                _context.HistoryAddSlots.Add(historyAddSlot);

                await _context.SaveChangesAsync();

                var message = Extension.CreateSlotMesseageWeeklyInMultiMonth(BaseAddressAddMulti, emailAdmin, accountDto, roomDto,
                            createAppointmentSlotAdvance.Description, createAppointmentSlotAdvance.Note, code, createAppointmentSlotAdvance.Start,
                            createAppointmentSlotAdvance.End, createAppointmentSlotAdvance.Title);

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
                        item.PatientName = createAppointmentSlotAdvance.Title;

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
                        TypedSubmit = "Create",
                        CodeId = code,
                        OldCodeId = code,
                        RoomId = createAppointmentSlotAdvance.Room,
                        Title = createAppointmentSlotAdvance.Title,
                        Editor = createAppointmentSlotAdvance.Email,
                        SendMail = false,
                    };
                    HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(historySubmit);

                    _context.HistoryAddSlots.Add(historyAddSlot);
                    await _context.SaveChangesAsync();
                }

                if (final.Count > 0)
                {
                    var message = Extension.CreateMessageAdvance(BaseAddressAddMulti, emailAdmin, accountDto, roomDto.Name,
                        createAppointmentSlotAdvance.Description, createAppointmentSlotAdvance.Note, code, createAppointmentSlotAdvance.Start, createAppointmentSlotAdvance.End, createAppointmentSlotAdvance.Title);

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
            });

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

        public async Task<Result> DeleteAppointmentBooked(Guid codeId)
        {
            Result rs = new();

            try
            {
                using var transaction = _context.Database.BeginTransaction();

                var history = await _context.HistoryAddSlots.SingleOrDefaultAsync(X => X.CodeId.Equals(codeId));
                _context.HistoryAddSlots.Remove(history);

                var slot = await _context.Appointments.Where(x => x.CodeId.Equals(codeId)).ToListAsync();
                foreach (var item in slot)
                {
                    item.PatientName = "";
                    item.PatientId = "";
                    item.Status = "free";
                    item.Description = "";
                    item.Email = "";
                    item.Note = "";
                    item.CodeId = Guid.Empty;
                }
                _context.UpdateRange(slot);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                rs.message = "Success";

                return rs;

            }
            catch (Exception ex)
            {
                rs.message = ex.Message;

                return rs;
            }
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
            if (doctor == 0)
            {
                var ap = await _context.Appointments.Where(e => !((e.End <= start) || (e.Start >= end))).Include(e => e.Room).AsNoTracking().ToListAsync();

                var apDto = _mapper.Map<List<AppointmentSlotDto>>(ap);

                return apDto;
            }
            else
            {
                var ap = await _context.Appointments.Where(e => e.Room.Id == doctor && !((e.End <= start) || (e.Start >= end))).Include(e => e.Room).AsNoTracking().ToListAsync();

                var apDto = _mapper.Map<List<AppointmentSlotDto>>(ap);

                return apDto;
            }

        }
        public async Task<Result> MailReplyAccepted(Guid fromId, Guid toId)
        {
            Result result = new();

            var history = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(toId));

            if (history == null || history.SendMail == true)
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

            UpdateHistoryAddSlotDto updateHistoryAddSlotDto = new();

            updateHistoryAddSlotDto.SendMail = false;

            updateHistoryAddSlotDto.DescStatus = "confirmed";

            history.Status = updateHistoryAddSlotDto.DescStatus;

            _context.HistoryAddSlots.Update(_mapper.Map(updateHistoryAddSlotDto, history));

            await _context.SaveChangesAsync();

            var message = Extension.ConfirmMoveSlotMesseageAccepted(baseAddress, emailAdmin, accountDto, roomDto.Name,
            historyDto.Description, historyDto.Note, historyDto.Start,
            historyDto.End, fromId, toId, history.Title);

            _emailSender.ReplyEmail(message, accountDto.Email, accountDto.accountDetailDto.FullName, emailAdmin);

            result.message = "Success";
            return result;
        }

        public async Task<Result> MailReplyRejected(Guid fromId, Guid toId)
        {
            Result result = new();

            var history = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(toId));

            if (history == null || history.SendMail == true)
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
            UpdateHistoryAddSlotDto updateHistoryAddSlotDto = new();

            updateHistoryAddSlotDto.SendMail = false;

            updateHistoryAddSlotDto.DescStatus = history.Status;

            _context.HistoryAddSlots.Update(_mapper.Map(updateHistoryAddSlotDto, history));

            await _context.SaveChangesAsync();
            var message = Extension.ConfirmMoveSlotMesseageRejected(baseAddress, emailAdmin, accountDto, roomDto.Name,
            historyDto.Description, historyDto.Note, historyDto.Start,
            historyDto.End, fromId, toId, historyDto.Title);

            _emailSender.ReplyEmail(message, accountDto.Email, accountDto.accountDetailDto.FullName, emailAdmin);
            result.message = "Success";

            return result;
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
            var historyAdd = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(appointmentSlot.CodeId));

            if (update.Status != historyAdd.Status && update.CustomAdd > 0)
            {
                result.message = "Vui lòng chọn lại thao tác!\nChỉ được chọn 1 trong 2: Cập nhật trạng thái hoặc dời lịch";
                return result;
            }
            var accountDb = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Contains(appointmentSlot.Email));

            var adminDb = await _context.Accounts.Include(x => x.AccountDetail).SingleOrDefaultAsync(x => x.Email.Contains(email));

            if (accountDb.Email == email || adminDb.AuthorizeRole.ToString() == "AdminRole" || adminDb.AuthorizeRole.ToString() == "ManagerRole")
            {
                var slot = await _context.Appointments.Where(x => x.Room.Id == update.Resource).ToListAsync();

                var account = _mapper.Map<AccountDto>(accountDb);

                var admin = _mapper.Map<AccountDto>(adminDb);

                var apDto = _mapper.Map<AppointmentSlotDto>(appointmentSlot);

                var newRoom = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == update.Resource);

                var oldRoom = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == historyAdd.RoomId);

                var newRoomDto = _mapper.Map<RoomDto>(newRoom);

                var oldRoomDto = _mapper.Map<RoomDto>(oldRoom);

                var checkExist = await _context.Appointments.Where(x => x.CodeId.Equals(apDto.CodeId)).OrderByDescending(x => x.Start).ToListAsync();

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

                if (update.CustomAdd == 0)
                {
                    result = await UpdateStatusSlot(result, update, apDto, newRoomDto, oldRoomDto, email, slot, account, admin, checkExist, historyAdd);

                }
                if (update.CustomAdd == 1)
                {

                    result = await CreateAppoinmentTypeFirst(result, update, apDto, newRoomDto, oldRoomDto, email, slot, account, admin, checkExist, historyAdd);

                }
                if (update.CustomAdd == 2)
                {
                    result = await CreateAppoinmentTypeSecond(result, update, apDto, newRoomDto, oldRoomDto, email, slot, account, admin, checkExist, historyAdd);

                }
                else if (update.CustomAdd == 3)
                {
                    result = await CreateAppoinmentTypeThird(result, update, apDto, newRoomDto, oldRoomDto, email, slot, account, admin, checkExist, historyAdd);

                }
                return result;

            }
            else
            {
                result.message = "Bạn không có quyền sửa";
                return result;
            }

        }

        public async Task<Result> CreateAppoinmentTypeFirst(Result result, AppointmentSlotUpdate update, AppointmentSlotDto appointmentSlotDto, RoomDto newRoomDto, RoomDto oldRoomDto,
                                                            string email, List<AppointmentSlot> slot, AccountDto account, AccountDto admin,
                                                            List<AppointmentSlot> checkExist, HistoryAddSlot historyAdd)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var dateForWeek = Extension.DaysWeakly(update.Start, update.End, update.CustomAdd);


                Guid codeId = Guid.NewGuid();

                CreateHistoryAddSlotDto createHistoryAddSlotDto = new()
                {
                    CodeId = codeId,
                    OldCodeId = appointmentSlotDto.CodeId,
                    TypedSubmit = "Edit",
                    Email = appointmentSlotDto.Email,
                    Description = update.Description,
                    Note = update.Note,
                    Status = "waiting",
                    RoomId = newRoomDto.Id,
                    Start = update.Start,
                    End = update.End,
                    Editor = email,
                    Title = update.Name,
                    SendMail = false,
                };
                HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(createHistoryAddSlotDto);


                _context.HistoryAddSlots.Add(historyAddSlot);

                await _context.SaveChangesAsync();

                foreach (var date in dateForWeek)
                {
                    var listSlot = Extension.SortList(date.Start, date.End, slot);

                    var existConfirmed = listSlot.Where(x => x.Status != "free").ToList();

                    if (existConfirmed != null && existConfirmed.Count > 0 && existConfirmed[0].CodeId.Equals(appointmentSlotDto.CodeId) == false)
                    {
                        result.message = "Khoảng thời gian bạn chọn đã được đặt";
                        return result;
                    }

                    else
                    {
                        listSlot.ForEach(item =>
                        {
                            item.PatientName = update.Name;


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

                if (admin.AuthorizeRole.ToString() == "AdminRole" || admin.AuthorizeRole.ToString() == "ManagerRole")
                {

                    var baseAddress = _configuration.GetValue<string>("BaseAddressChangeTime");

                    var message = Extension.MoveSlotMesseageWeeklyInMonth(baseAddress, email, account, oldRoomDto, newRoomDto, update.Name, update.Description, update.Note, appointmentSlotDto.CodeId, codeId, historyAdd.Start, historyAdd.End, historyAddSlot.Start, historyAddSlot.End);

                    _emailSender.ReplyEmail(message, email, admin.accountDetailDto.FullName, account.Email);
                }


                if (checkExist.Count > 0)
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

                //Người dùng chỉnh sửa xóa lịch vừa đặt

                if (admin.AuthorizeRole.ToString() != "AdminRole" && admin.AuthorizeRole.ToString() != "ManagerRole")
                {
                    var historyPrevious = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(appointmentSlotDto.CodeId));

                    _context.HistoryAddSlots.Remove(historyPrevious);

                    await _context.SaveChangesAsync();

                    var emailAdmin = _configuration.GetValue<string>("EmailConfiguration:To");

                    var baseAddress = _configuration.GetValue<string>("BaseAddress");

                    var message = Extension.UserMoveSlotMesseageWeeklyInMonth(baseAddress, emailAdmin, account, oldRoomDto, newRoomDto, update.Name, update.Description, update.Note, codeId, codeId, historyAdd.Start, historyAdd.End, historyAddSlot.Start, historyAddSlot.End);

                    _emailSender.ReplyEmail(message, account.Email, account.accountDetailDto.FullName, emailAdmin);
                }
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
        public async Task<Result> CreateAppoinmentTypeSecond(Result result, AppointmentSlotUpdate update, AppointmentSlotDto apDto, RoomDto newRoomDto, RoomDto oldRoomDto,
                                                    string email, List<AppointmentSlot> slot, AccountDto account, AccountDto admin,
                                                    List<AppointmentSlot> checkExist, HistoryAddSlot historyAdd)
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
                    Email = apDto.Email,
                    Description = update.Description,
                    Note = update.Note,
                    Status = "waiting",
                    RoomId = newRoomDto.Id,
                    Start = update.Start,
                    End = update.End,
                    OldCodeId = apDto.CodeId,
                    TypedSubmit = "Edit",
                    Editor = email,
                    Title = update.Name,
                    SendMail = false,
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
                            item.PatientName = update.Name;


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
                if (admin.AuthorizeRole.ToString() == "AdminRole" || admin.AuthorizeRole.ToString() == "ManagerRole")
                {
                    var baseAddress = _configuration.GetValue<string>("BaseAddressChangeTime");

                    var message = Extension.MoveSlotMesseageWeeklyInMultiMonth(baseAddress, email, account, oldRoomDto, newRoomDto, update.Description, update.Note, apDto.CodeId, codeId, historyAdd.Start, historyAdd.End, historyAddSlot.Start, historyAddSlot.End, update.Name);


                    _emailSender.ReplyEmail(message, email, admin.accountDetailDto.FullName, account.Email);
                }


                if (checkExist.Count > 0)
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

                //Người dùng chỉnh sửa xóa lịch vừa đặt

                if (admin.AuthorizeRole.ToString() != "AdminRole" && admin.AuthorizeRole.ToString() != "ManagerRole")
                {
                    var historyPrevious = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(apDto.CodeId));

                    _context.HistoryAddSlots.Remove(historyPrevious);

                    await _context.SaveChangesAsync();

                    var emailAdmin = _configuration.GetValue<string>("EmailConfiguration:To");

                    var baseAddress = _configuration.GetValue<string>("BaseAddress");

                    var message = Extension.UserMoveSlotMesseageWeeklyInMultiMonth(baseAddress, emailAdmin, account, oldRoomDto, newRoomDto, update.Description, update.Note, codeId, codeId, historyAdd.Start, historyAdd.End, historyAddSlot.Start, historyAddSlot.End, update.Name);

                    _emailSender.ReplyEmail(message, account.Email, account.accountDetailDto.FullName, emailAdmin);

                }
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
        public async Task<Result> CreateAppoinmentTypeThird(Result result, AppointmentSlotUpdate update, AppointmentSlotDto apDto, RoomDto newRoomDto, RoomDto oldRoomDto,
                                            string email, List<AppointmentSlot> slot, AccountDto account, AccountDto admin,
                                            List<AppointmentSlot> checkExist, HistoryAddSlot historyAdd)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                var final = Extension.SortList(update.Start, update.End, slot);

                var exist = final.Where(x => x.Status != "free").ToList();

                if (exist != null && exist.Count > 0)
                {
                    result.message = "Khoảng thời gian bạn chọn đã được đặt";
                    return result;
                }

                apDto.PatientName = update.Name;

                Guid codeId = Guid.NewGuid();



                final.ForEach(item =>
                {
                    item.PatientName = apDto.PatientName;


                    item.PatientId = "d2cdd687-3f3d-d71b-258a-06906cb82f44";

                    item.Status = "waiting";

                    item.Email = account.Email;

                    item.Description = update.Description;

                    item.Note = update.Note;

                    item.CodeId = codeId;

                    _context.Appointments.Update(item);

                });

                await _context.SaveChangesAsync();
                CreateHistoryAddSlotDto createHistoryAddSlotDto = new()
                {
                    CodeId = codeId,
                    Email = apDto.Email,
                    Description = update.Description,
                    Note = update.Note,
                    Status = "waiting",
                    RoomId = newRoomDto.Id,
                    Start = update.Start,
                    End = update.End,
                    OldCodeId = apDto.CodeId,
                    TypedSubmit = "Edit",
                    Editor = email,
                    Title = update.Name,
                    SendMail = false,
                };

                HistoryAddSlot historyAddSlot = _mapper.Map<HistoryAddSlot>(createHistoryAddSlotDto);

                _context.HistoryAddSlots.Add(historyAddSlot);

                await _context.SaveChangesAsync();

                if (admin.AuthorizeRole.ToString() == "AdminRole" || admin.AuthorizeRole.ToString() == "ManagerRole")
                {
                    var baseAddress = _configuration.GetValue<string>("BaseAddressChangeTime");

                    var message = Extension.MoveSlotMesseage(baseAddress, email, account, oldRoomDto, newRoomDto, update.Description, update.Note, apDto.CodeId, codeId, historyAdd.Start, historyAdd.End, historyAddSlot.Start, historyAddSlot.End, update.Name);

                    _emailSender.ReplyEmail(message, email, admin.accountDetailDto.FullName, account.Email);
                }


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

                //Người dùng chỉnh sửa xóa lịch vừa đặt
                if (admin.AuthorizeRole.ToString() != "AdminRole" && admin.AuthorizeRole.ToString() != "ManagerRole")
                {
                    var historyPrevious = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(apDto.CodeId));

                    _context.HistoryAddSlots.Remove(historyPrevious);

                    await _context.SaveChangesAsync();
                    var emailAdmin = _configuration.GetValue<string>("EmailConfiguration:To");

                    var baseAddress = _configuration.GetValue<string>("BaseAddress");

                    var message = Extension.UserMoveSlotMesseage(baseAddress, emailAdmin, account, oldRoomDto, newRoomDto, update.Description, update.Note, codeId, codeId, historyAdd.Start, historyAdd.End, historyAddSlot.Start, historyAddSlot.End, update.Name);

                    _emailSender.ReplyEmail(message, account.Email, account.accountDetailDto.FullName, emailAdmin);
                }
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

        public async Task<Result> UpdateStatusSlot(Result result, AppointmentSlotUpdate update, AppointmentSlotDto apDto, RoomDto newRoomDto, RoomDto oldRoomDto,
                                    string email, List<AppointmentSlot> slot, AccountDto account, AccountDto admin,
                                    List<AppointmentSlot> checkExist, HistoryAddSlot historyAdd)
        {
            try
            {


                if (update.Status == "waiting")
                {
                    result = await CreateAppoinmentTypeThird(result, update, apDto, newRoomDto, oldRoomDto, email, slot, account, admin, checkExist, historyAdd);
                    return result;

                }
                using var transaction = _context.Database.BeginTransaction();

                var checkExistType3 = await _context.Appointments.Where(x => x.CodeId.Equals(apDto.CodeId)).OrderByDescending(x => x.Start).ToListAsync();

                var history = await _context.HistoryAddSlots.Where(x => x.CodeId.Equals(apDto.CodeId)).OrderByDescending(x => x.Start).ToListAsync();

                var historyOld = await _context.HistoryAddSlots.FirstOrDefaultAsync(x => x.CodeId.Equals(history[0].OldCodeId) == true && x.OldCodeId.Equals(history[0].OldCodeId) == true && x.TypedSubmit != "Create");

                if (historyOld != null)
                {
                    _context.HistoryAddSlots.Remove(historyOld);

                    await _context.SaveChangesAsync();
                }


                checkExistType3.ForEach(ce =>
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
                    var message = Extension.ConfirmSlotMesseageAccepted(email, account, apDto.DoctorName, apDto.Description, apDto.Note, checkExist[checkExist.Count - 1].Start, checkExist[0].End, apDto.PatientName);

                    _emailSender.ReplyEmail(message, email, admin.accountDetailDto.FullName, account.Email);
                }
                result.message = "Cập nhật thành công!";
                return result;
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                return result;
            }

        }

        public async Task<AppointmentSlotDto> GetAppointmentSlot(int id)
        {
            var appointmentSlot = await _context.Appointments.Include(x => x.Room).SingleOrDefaultAsync(x => x.Id == id);

            if (appointmentSlot == null)
            {
                return default;
            }
            var appointmentSlotDto = _mapper.Map<AppointmentSlotDto>(appointmentSlot);

            return appointmentSlotDto;
        }

        public async Task<IEnumerable<HistoryAddSlotExport>> ExportHistoryAllSlotDtos(DateTime fromDate, DateTime toDate, bool confirmed, int room, string email)
        {
            List<HistoryAddSlot> report = new();
            var start = DateTime.Parse(fromDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            var to = DateTime.Parse(toDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
            {
                report = await _context.HistoryAddSlots.Where(x => x.Start >= start && x.End <= to).AsNoTracking().ToListAsync();
            }
            else
            {
                if (fromDate == DateTime.MinValue)
                {

                    report = await _context.HistoryAddSlots.Where(x => x.End <= to).AsNoTracking().ToListAsync();
                }
                if (toDate == DateTime.MinValue)
                {
                    report = await _context.HistoryAddSlots.Where(x => x.Start >= start).AsNoTracking().ToListAsync();
                }
            }
            if (confirmed == true)
            {
                report = report.Where(x => x.Status == "confirmed").ToList();
            }
            if (room > 0)
            {
                report = report.Where(x => x.RoomId == room).ToList();

            }
            if (email != null)
            {
                report = report.Where(x => x.Email == email).ToList();

            }
            //if (!string.IsNullOrEmpty(passed.ToString()))
            //{

            //    report = await _context.ReportUserScoreMonthlies.OrderByDescending(x => x.CompletedDate).Where(x => x.Passed == passed).ToListAsync();
            //}
            var reportDto = _mapper.Map<List<HistoryAddSlotDto>>(report);

            var historyExport = new List<HistoryAddSlotExport>();
            foreach (var reportItem in reportDto)
            {
                var nameQuiz = await _roomService.GetRoomByid(reportItem.RoomId);

                historyExport.Add(new HistoryAddSlotExport()
                {
                    Id = reportItem.Id,
                    Title = reportItem.Title,
                    Room = nameQuiz.Name,

                    Email = reportItem.Email,
                    Editor = reportItem.Editor,
                    Description = reportItem.Description,

                    Note = reportItem.Note,

                    Start = reportItem.Start,

                    End = reportItem.End,
                    SendMail = reportItem.SendMail,

                    TypedSubmit = reportItem.TypedSubmit,
                    Status = reportItem.Status,
                });
            }
            return historyExport;
        }
    }
}
