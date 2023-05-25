

using Microsoft.Extensions.Configuration;
using System;
using System.Security.Principal;
using WebLearning.Application.Email;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.Room;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.BookingCalender
{
    public class DateAllWeek
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
    public class FreeSlotInMonth
    {
        public List<AppointmentSlot> TotalSlotInMonth { get; set; }
        public List<AppointmentSlot> TotalSlotFreeBeforeDateNow { get; set; }
        public List<AppointmentSlot> TotalSlotFreeBetweenMinAndMaxDate { get; set; }

        public List<AppointmentSlot> SlotFreeInBusyTimeAndGreateThanDateNow { get; set; }

    }
    public static class Extension
    {
        public static List<AppointmentSlot> SortList(DateTime fromDate, DateTime toDate, List<AppointmentSlot> slot)
        {
            List<AppointmentSlot> slotListStart = new();
            List<AppointmentSlot> final = new();
            foreach (var START in slot)
            {
                int s = DateTime.Compare(fromDate, START.Start);
                if (s < 0 || s == 0)
                {
                    slotListStart.Add(START);
                };
                if (fromDate.Date == START.Start.Date && fromDate.Minute >= 1 && fromDate.Minute < 60 && fromDate.Minute != 30 && fromDate.Hour == START.Start.Hour && fromDate.TimeOfDay < START.End.TimeOfDay)
                {
                    if (s > 0 || s == 0)
                    {
                        slotListStart.Add(START);
                    };
                }

            }
            foreach (var i in slotListStart)
            {
                int e = DateTime.Compare(i.End, toDate);

                if (e < 0 || e == 0)
                {
                    final.Add(i);
                };
                if (toDate.Date == i.End.Date && toDate.Minute >= 1 && toDate.Minute < 60 && toDate.Minute != 30 && toDate.Hour == i.Start.Hour && toDate.TimeOfDay > i.Start.TimeOfDay)
                {
                    if (e > 0 || e == 0)
                    {
                        final.Add(i);
                    };
                }


            }
            return final;
        }
        public static Message MoveSlotMesseageWeeklyInMonth(string baseAddress,string emailAdmin, AccountDto accountBooker, RoomDto roomOld,RoomDto roomNew, string desctiption, string note, 
                                                        Guid codeOld, Guid codeNew, DateTime fromDateOld, DateTime toDateOLd, DateTime fromDateNew, DateTime toDateNew)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "HÀNG TUẦN TRONG THÁNG</h1>\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, "+accountBooker.accountDetailDto.FullName+"</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn có một yêu cầu dời lịch từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +
                                                        "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + emailAdmin + ">" + emailAdmin + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.accountDetailDto.FullName + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.Email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +


                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:16px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt cũ:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomOld.Name + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDateOld+ "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDateOLd + "</b></p>\r\n" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:16px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt mới:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomNew.Name + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDateNew + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDateNew + "</b></p>\r\n" +

                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #5cb85c;color:#fff;margin-bottom:17px\"" +
                            "href=" + baseAddress + codeOld + "/" + codeNew + "/accepted>" +
                            "Đồng ý</a>" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #d9534f;color:#fff;margin-bottom:17px;margin-left:10px\"" +
                            "href=" + baseAddress + codeOld + "/" + codeNew + "/rejected>" +
                            "Từ chối</a>" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t<br>\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n</div>\r\n" +
                            "</td>\r\n</tr>\r\n</tbody></table>\r\n</td>\r\n</tr>\r\n" +
                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n"+
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "THÔNG BÁO DỜI LỊCH", $"{body}");

            return message;

        }
        public static Message MoveSlotMesseageWeeklyInMultiMonth(string baseAddress, string emailAdmin, AccountDto accountBooker, RoomDto roomOld, RoomDto roomNew, string desctiption, string note,
                                                        Guid codeOld, Guid codeNew, DateTime fromDateOld, DateTime toDateOLd, DateTime fromDateNew, DateTime toDateNew)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "HÀNG TUẦN TRONG NHIỀU THÁNG</h1>\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + accountBooker.accountDetailDto.FullName + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn có một yêu cầu dời lịch từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +
                                                        "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + emailAdmin + ">" + emailAdmin + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.accountDetailDto.FullName + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.Email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +


                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:14px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt cũ:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomOld.Name + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDateOld + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDateOLd + "</b></p>\r\n" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:14px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;text-decoration:underline;margin-top:0;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt mới:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomNew.Name + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDateNew + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDateNew + "</b></p>\r\n" +

                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #5cb85c;color:#fff;margin-bottom:17px\"" +
                            "href=" + baseAddress + codeOld + "/" + codeNew + "/accepted>" +
                            "Đồng ý</a>" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #d9534f;color:#fff;margin-bottom:17px;margin-left:10px\"" +
                            "href=" + baseAddress + codeOld + "/" + codeNew + "/rejected>" +
                            "Từ chối</a>" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t<br>\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n</div>\r\n" +
                            "</td>\r\n</tr>\r\n</tbody></table>\r\n</td>\r\n</tr>\r\n" +
                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "THÔNG BÁO DỜI LỊCH", $"{body}");

            return message;

        }
        public static Message MoveSlotMesseage(string baseAddress, string emailAdmin, AccountDto accountBooker, RoomDto roomOld, RoomDto roomNew, string desctiption, string note,
                                                        Guid codeOld, Guid codeNew, DateTime fromDateOld, DateTime toDateOLd, DateTime fromDateNew, DateTime toDateNew)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>"); var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + accountBooker.accountDetailDto.FullName + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn có một yêu cầu dời lịch từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +
                                                        "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + emailAdmin + ">" + emailAdmin + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.accountDetailDto.FullName + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.Email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +


                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:12px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt cũ:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomOld.Name + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDateOld + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDateOLd + "</b></p>\r\n" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:12px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt mới:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomNew.Name + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDateNew + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDateNew + "</b></p>\r\n" +

                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #5cb85c;color:#fff;margin-bottom:17px\"" +
                            "href=" + baseAddress + codeOld + "/" + codeNew + "/accepted>" +
                            "Đồng ý</a>" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #d9534f;color:#fff;margin-bottom:17px;margin-left:10px\"" +
                            "href=" + baseAddress + codeOld + "/" + codeNew + "/rejected>" +
                            "Từ chối</a>" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t<br>\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n</div>\r\n" +
                            "</td>\r\n</tr>\r\n</tbody></table>\r\n</td>\r\n</tr>\r\n" +
                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "THÔNG BÁO DỜI LỊCH", $"{body}");

            return message;

        }

        public static Message ConfirmSlotMesseageAccepted(string emailAdmin, AccountDto accountBooker, string roomName, string desctiption, string note,
                                                DateTime fromDate, DateTime toDate)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                                                        "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "PHẢN HỒI LỊCH ĐẶT</h1>\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + accountBooker.accountDetailDto.FullName + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn đã được phản hồi lịch đặt từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +
                             "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + emailAdmin + ">" + emailAdmin + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.accountDetailDto.FullName + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.Email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:12px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt phòng:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomName + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Trạng thái: <span style = 'background-color:#5cb85c;display: inline;padding: .3em .7em .4em;font-size: 75%;font-weight: 700;line-height: 1;color: #fff;text-align: center;white-space: nowrap;vertical-align: baseline;border-radius: .25em;' > " +
                                    "Duyệt thành công" +
                                    "</span>" +"<br>\r\n" +

                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "PHẢN HỒI LỊCH ĐẶT", $"{body}");

            return message;

        }
        public static Message ConfirmMoveSlotMesseageAccepted(string baseAddress, string emailAdmin, AccountDto accountBooker, string roomName, string desctiption, string note,
                                        DateTime fromDate, DateTime toDate, Guid fromId, Guid toId)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                                                        "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "PHẢN HỒI DỜI LỊCH ĐẶT</h1>\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + emailAdmin + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn đã được phản hồi dời lịch đặt từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +

                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + accountBooker.Email + ">" + accountBooker.Email + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + emailAdmin + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + emailAdmin + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:12px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt phòng:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomName + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Trạng thái: <span style = 'background-color:#5cb85c;display: inline;padding: .3em .7em .4em;font-size: 75%;font-weight: 700;line-height: 1;color: #fff;text-align: center;white-space: nowrap;vertical-align: baseline;border-radius: .25em;' > " +
                                    "Đồng ý" +
                                    "</span>" + "<br><br/>\r\n" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #5cb85c;color:#fff;margin-bottom:17px\"" +
                            "href=" + baseAddress + fromId + "/" + toId + "/trang-thai=1/accepted>" +
                            "Duyệt</a>" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #d9534f;color:#fff;margin-bottom:17px;margin-left:10px\"" +
                            "href=" + baseAddress + fromId + "/" + toId + "/trang-thai=1/rejected>" +
                            "Từ chối</a>" +
                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "PHẢN HỒI DỜI LỊCH", $"{body}");

            return message;

        }
        public static Message ConfirmMoveSlotMesseageRejected(string baseAddress, string emailAdmin, AccountDto accountBooker, string roomName, string desctiption, string note,
                                        DateTime fromDate, DateTime toDate, Guid fromId, Guid toId)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                                                        "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "PHẢN HỒI DỜI LỊCH ĐẶT</h1>\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + emailAdmin + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn đã được phản hồi dời lịch đặt từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +

                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + accountBooker.Email + ">" + accountBooker.Email + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + emailAdmin + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + emailAdmin + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:12px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt phòng:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomName + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Trạng thái: <span style = 'background-color:#d9534f;display: inline;padding: .3em .7em .4em;font-size: 75%;font-weight: 700;line-height: 1;color: #fff;text-align: center;white-space: nowrap;vertical-align: baseline;border-radius: .25em;' > " +
                                    "Từ chối" +
                                    "</span>" + "<br><br/>\r\n" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #d9534f;color:#fff;margin-bottom:17px;margin-left:10px\"" +
                            "href=" + baseAddress + fromId + "/" + toId + "/trang-thai=1/rejected>" +
                            "Hủy lịch đặt</a>" +
                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "PHẢN HỒI DỜI LỊCH", $"{body}");

            return message;

        }
        public static Message ConfirmSlotMesseageRejected(string emailAdmin, AccountDto accountBooker, string roomName, string desctiption, string note,
                                        DateTime fromDate, DateTime toDate)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                                                        "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "TỪ CHỐI LỊCH ĐẶT</h1>\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + accountBooker.accountDetailDto.FullName + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn đã được phản hồi lịch đặt từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +

                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + emailAdmin + ">" + emailAdmin + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.accountDetailDto.FullName + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.Email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:12px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt phòng:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomName + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Trạng thái: <span style = 'background-color:#d9534f;display: inline;padding: .3em .7em .4em;font-size: 75%;font-weight: 700;line-height: 1;color: #fff;text-align: center;white-space: nowrap;vertical-align: baseline;border-radius: .25em;' > " +
                                    "Từ chối" +
                                    "</span>" + "<br>\r\n" +

                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "PHẢN HỒI LỊCH ĐẶT", $"{body}");

            return message;

        }
        public static Message CreateMessageSingleSlot(string baseAddress, string emailAdmin, AccountDto accountBooker, string roomName, string desctiption, string note, AppointmentSlot appointmentSlotDto,
                                        Guid codeId, DateTime fromDate, DateTime toDate)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + emailAdmin + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#9A0000\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn có một yêu cầu đặt lịch từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + accountBooker.Email + ">" + accountBooker.Email + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.accountDetailDto.FullName + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.Email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:14px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;margin-bottom:4px;font-size:16px;text-decoration:underline;line-height:18px;color:#71787e\">" +
                            "Lịch đặt phòng:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomName + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDate + "</b></p>\r\n" +



                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #5cb85c;color:#fff;margin-bottom:17px\"" +
                            "href=" + baseAddress + appointmentSlotDto.CodeId + "/" + appointmentSlotDto.CodeId + "/trang-thai=3/accepted>" +
                            "Duyệt</a>" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #d9534f;color:#fff;margin-bottom:17px;margin-left:10px\"" +
                            "href=" + baseAddress + appointmentSlotDto.CodeId + "/" + appointmentSlotDto.CodeId + "/trang-thai=3/rejected>" +
                            "Từ chối</a>" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t<br>\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n</div>\r\n" +
                            "</td>\r\n</tr>\r\n</tbody></table>\r\n</td>\r\n</tr>\r\n" +
                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "ĐẶT LỊCH HỌP", $"{body}");

            return message;

        }
        public static Message CreateMessageAdvance(string baseAddress, string emailAdmin, AccountDto accountBooker, string roomName, string desctiption, string note,
                                Guid codeId, DateTime fromDate, DateTime toDate)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + emailAdmin + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn có một yêu cầu đặt lịch từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + emailAdmin + ">" + emailAdmin + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.accountDetailDto.FullName + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.Email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:12px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt phòng:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + roomName + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDate + "</b></p>\r\n" +



                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #5cb85c;color:#fff;margin-bottom:17px\"" +
                            "href=" + baseAddress +codeId + "/" + codeId + "/trang-thai=3/accepted>" +
                            "Duyệt</a>" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #d9534f;color:#fff;margin-bottom:17px;margin-left:10px\"" +
                            "href=" + baseAddress + codeId + "/" + codeId + "/trang-thai=3/rejected>" +
                            "Từ chối</a>" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t<br>\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n</div>\r\n" +
                            "</td>\r\n</tr>\r\n</tbody></table>\r\n</td>\r\n</tr>\r\n" +
                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "ĐẶT LỊCH HỌP", $"{body}");

            return message;

        }
        public static Message CreateSlotMesseageWeeklyInMonth(string BaseAddressAddMulti, string emailAdmin, AccountDto accountBooker, RoomDto room, string desctiption, string note,
                                                            Guid code, DateTime fromDate, DateTime toDate)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "HÀNG TUẦN TRONG THÁNG</h1>\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + accountBooker.accountDetailDto.FullName + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn có một yêu cầu đặt lịch  từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +
                             
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + accountBooker.Email + ">" + accountBooker.Email + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.accountDetailDto.FullName + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.Email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:12px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt phòng:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + room.Name + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDate + "</b></p>\r\n" +

                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #5cb85c;color:#fff;margin-bottom:17px\"" +
                            "href=" + BaseAddressAddMulti + code + "/" + code + "/trang-thai=3/accepted>" +
                            "Đồng ý</a>" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #d9534f;color:#fff;margin-bottom:17px;margin-left:10px\"" +
                            "href=" + BaseAddressAddMulti + code + "/" + code + "/trang-thai=3/rejected>" +
                            "Từ chối</a>" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t<br>\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n</div>\r\n" +
                            "</td>\r\n</tr>\r\n</tbody></table>\r\n</td>\r\n</tr>\r\n" +
                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "ĐẶT LỊCH HỌP", $"{body}");

            return message;

        }
        public static Message CreateSlotMesseageWeeklyInMultiMonth(string BaseAddressAddMulti, string emailAdmin, AccountDto accountBooker, RoomDto room, string desctiption, string note,
                                                    Guid code, DateTime fromDate, DateTime toDate)
        {
            string descriptionReplace = desctiption.Replace("\n", "<br>");
            string noteReplace = note.Replace("\n", "<br>");
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://i.imgur.com/WIA2pTM.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "HÀNG TUẦN TRONG NHIỀU THÁNG</h1>\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #9a0000;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào, " + accountBooker.accountDetailDto.FullName + "</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#121e28\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn có một yêu cầu đặt lịch  từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\" href=mailto:" + accountBooker.Email + ">" + accountBooker.Email + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Người đặt: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.accountDetailDto.FullName + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Email: </a></b>" + "<span style=\"color: #9A0000;font-size:14px\"> " + accountBooker.Email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"<a style=\"font-size:14px;color: #9A0000\">Nội dung: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + descriptionReplace + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #9A0000\"><a style=\"font-size:14px;color: #9A0000\">Ghi chú: </a></b><br>" + "<span style=\"color: #9A0000;font-size:14px\"> " + noteReplace + "</span><br><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:12px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;text-decoration:underline;margin-bottom:4px;font-size:16px;line-height:18px;color:#71787e\">" +
                            "Lịch đặt phòng:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Phòng: <b style=\"color:red\">" + room.Name + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian bắt dầu: <b style=\"color:red\">" + fromDate + "</b></p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian kết thúc: <b style=\"color:red\">" + toDate + "</b></p>\r\n" +

                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #5cb85c;color:#fff;margin-bottom:17px\"" +
                            "href=" + BaseAddressAddMulti + code + "/" + code + "/trang-thai=3/accepted>" +
                            "Đồng ý</a>" +
                            "<a style=\"font-size:14px;line-height:24px;padding:8px 20px;font-weight:bold;text-decoration:none;margin-bottom:0;display:inline-block;background: #d9534f;color:#fff;margin-bottom:17px;margin-left:10px\"" +
                            "href=" + BaseAddressAddMulti + code + "/" + code + "/trang-thai=3/rejected>" +
                            "Từ chối</a>" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t<br>\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n</div>\r\n" +
                            "</td>\r\n</tr>\r\n</tbody></table>\r\n</td>\r\n</tr>\r\n" +
                            "<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;background:#fafbfb;padding:3% 5%\">\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ VXH.Booking</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">VXH.Booking giúp bạn đăng kí phòng họp trong vài phút. Cho dù bạn đang ở văn phòng, ở nhà, đang đi trên đường, VXH.Booking sẽ cung cấp giải pháp và dịch vụ đến với bạn.</p>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">CÂU HỎI VỀ TÀI LIỆU</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:0\">Nếu bạn cần sửa đổi hoặc có câu hỏi về nội dung trong tài liệu cũng như đặt lịch, vui lòng liên hệ với người gửi bằng cách gửi email trực tiếp cho họ.</p>\r\n</td>\r\n</tr>\r\n\r\n\r\n</tbody></table>\r\n\r\n\r\n</div>\r\n</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n</tr>\r\n</tbody></table>" +
                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{accountBooker.Email}" }, "ĐẶT LỊCH HỌP", $"{body}");

            return message;

        }
        public static List<DateAllWeek> DaysWeakly(DateTime fromDate, DateTime toDate, int custom)
        {
            List<DateTime> dayFroms = new();
            List<DateTime> dayTos = new();
            List<DateAllWeek> dateAllWeeks = new();

            if (custom == 1)
            {
                dayFroms.Add(fromDate);
                dayTos.Add(toDate);

                var dayInMonthFrom = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
                var dayInMonthTo = DateTime.DaysInMonth(toDate.Year, toDate.Month);

                DateTime lastMonthFrom = new(fromDate.Year, fromDate.Month, dayInMonthFrom, fromDate.Hour, fromDate.Minute, fromDate.Second);
                DateTime lastMonthTo = new(toDate.Year, toDate.Month, dayInMonthTo, toDate.Hour, toDate.Minute, toDate.Second);

                DateTime dayFrom = fromDate.AddDays(7);
                DateTime dayTo = toDate.AddDays(7);

                int checkDayInMonthFrom = DateTime.Compare(dayFrom, lastMonthFrom);
                int checkDayInMonthTo = DateTime.Compare(dayTo, lastMonthTo);

                if (checkDayInMonthFrom < 0 || checkDayInMonthFrom == 0)
                {
                    dayFroms.Add(dayFrom);

                }
                if (checkDayInMonthTo < 0 || checkDayInMonthTo == 0)
                {
                    dayTos.Add(dayTo);

                }
                for (int i = 0; i <= 4; i++)
                {

                    dayFrom = dayFrom.AddDays(7);
                    dayTo = dayTo.AddDays(7);
                    int checkDayInMonthFromAfterAdd = DateTime.Compare(dayFrom, lastMonthFrom);
                    int checkDayInMonthToAfterAdd = DateTime.Compare(dayTo, lastMonthTo);

                    if (checkDayInMonthFromAfterAdd < 0 || checkDayInMonthFromAfterAdd == 0)
                    {
                        dayFroms.Add(dayFrom);
                    }
                    if (checkDayInMonthToAfterAdd < 0 || checkDayInMonthToAfterAdd == 0)
                    {
                        dayTos.Add(dayTo);
                    }
                }

            }
            if (custom == 2)
            {
                dayFroms.Add(fromDate);

                DateTime newDateTo = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, toDate.Hour, toDate.Minute, toDate.Second);

                dayTos.Add(newDateTo);

                DateTime dayFrom = fromDate.AddDays(7);

                dayFroms.Add(dayFrom);

                DateTime dayTo = newDateTo.AddDays(7);

                dayTos.Add(dayTo);


                for (int i = 0; i <= (toDate - fromDate).Days; i++)
                {

                    dayFrom = dayFrom.AddDays(7);
                    dayTo = dayTo.AddDays(7);

                    int checkDayInMonthAfterAdd = DateTime.Compare(dayTo, toDate);

                    if (checkDayInMonthAfterAdd <= 0)
                    {
                        dayFroms.Add(dayFrom);
                        dayTos.Add(dayTo);

                    }

                    int checkDayInListDateTo = DateTime.Compare(dayTo, toDate);

                    if (checkDayInListDateTo > 0) break;
                }

            }
            for (int i = 0; i < dayFroms.Count; i++)
            {
                for (int j = i; j < dayTos.Count;)
                {
                    dateAllWeeks.Add(new DateAllWeek
                    {
                        Start = dayFroms[i],
                        End = dayTos[j],
                    });

                    break;
                }
            }
            return dateAllWeeks;
        }

        public static FreeSlotInMonth CheckFreeSlot(List<AppointmentSlot> listSlotFree, List<AppointmentSlot> listBusySlotInRoom, List<AppointmentSlot> listSlotBeforeDateStart,
                                                        List<AppointmentSlot> listSlotAfterDateStart, AppointmentSlotRange range)
        {
            FreeSlotInMonth freeSlotInMonth = new();

            if (listSlotFree.Count == 0)
            {
                if (listBusySlotInRoom.Count == 0)
                {
                    range.Start = DateTime.Now;

                    range.End = range.Start.AddMonths(3);
                    var slots = Timeline.GenerateSlots(range.Start, range.End, range.Scale);

                    freeSlotInMonth.TotalSlotInMonth = slots;

                    return freeSlotInMonth;
                }
                else
                {
                    range.Start = listBusySlotInRoom[0].End;
                    DateTime maxBusyStart = listBusySlotInRoom[0].End;

                    DateTime minBusyStart = listBusySlotInRoom[listBusySlotInRoom.Count() - 1].Start;

                    int checkGreaterThanNow = DateTime.Compare(maxBusyStart, DateTime.Now);
                    int checkLessThanNow = DateTime.Compare(minBusyStart, DateTime.Now);
                    int checkLessThanMax = DateTime.Compare(minBusyStart, maxBusyStart);

                    if (listSlotBeforeDateStart != null)
                    {
                        range.Start = listSlotBeforeDateStart[0].End;
                    }
                    range.End = range.Start.AddMonths(3);
                    List<AppointmentSlot> sp = listBusySlotInRoom;
                    var slots = Timeline.GenerateSlots(range.Start, range.End, range.Scale);
                    int compare = DateTime.Compare(range.Start, DateTime.Now);
                    if (compare >= 0)
                    {
                        var slotPrevios = Timeline.GenerateSlots(DateTime.Now, minBusyStart, range.Scale);

                        freeSlotInMonth.TotalSlotFreeBeforeDateNow = slotPrevios;
                    }

                    freeSlotInMonth.TotalSlotInMonth = slots;


                    return freeSlotInMonth;
                }

            }
            else
            {
                range.Start = listSlotFree[0].End;
                range.End = range.Start.AddMonths(3);
                var slots = Timeline.GenerateSlots(range.Start, range.End, range.Scale);

                freeSlotInMonth.TotalSlotInMonth = slots;
                return freeSlotInMonth;
            }
        }
    }
}
