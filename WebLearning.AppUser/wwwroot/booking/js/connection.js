var ba = null;
var em = null;
// get urlApi and username
var url = "/Booking/Address";
$.get(url, null, function (data) {
    ba = data;
});
var url = "/Booking/Name";
$.get(url, null, function (us) {
    em = us;
});



//--Load booking recently and list room >
$(document).ready(function () {
    $.ajax({
        url: '/dat-thanh-cong.html',
        datatype: "json",
        type: "GET",
        //data: formData,
        success: function (results) {
            $("#records_table").html("");
            $("#records_table").html(results);
        },
        //error: function (xhr) {
        //    alert('Vui lòng đăng nhập lại');
        //    window.location.href = "/dang-nhap.html"
        //}
    });
});
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: `${window.location.origin}/danh-sach-phong`,
        data: "{}",
        success: function (data) {
            var s = '<option value="0">--Chọn phòng--</option>';
            for (var i = 0; i < data.length; i++) {
                s += '<option value="' + data[i].id + '">' + data[i].name + '</option>';
            }
            $("#room").html(s);
        }
    });
});