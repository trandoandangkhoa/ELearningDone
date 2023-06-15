// ------------------Chi tiết tài sản JS-----------------------
var submitUpdateButton = document.getElementById('updatebutton');
var submitDeleteButton = document.getElementById('deletebutton');

let buttonUpdateClicked = false;
let buttonDeleteClicked = false;

submitUpdateButton.addEventListener('click', function handleClick() {
    $(".updateContainer").css("display", "block");
    submitUpdateButton.textContent = 'Hủy thao tác cập nhật';
    if (buttonUpdateClicked) {
        $(".updateContainer").css("display", "none");
        submitUpdateButton.textContent = 'Cập nhật';
        submitUpdateButton.innerHTML = `<i class="fa fa-edit"></i> Cập nhật`;

        buttonUpdateClicked = false;
        return;
    }
    buttonUpdateClicked = true;

});
submitDeleteButton.addEventListener('click', function handleClick() {
    $(".deleteContainer").css("display", "block");
    submitDeleteButton.textContent = 'Hủy thao tác xóa';


    if (buttonDeleteClicked) {
        $(".deleteContainer").css("display", "none");
        submitDeleteButton.innerHTML = `<i class="fa fa-trash"></i> Xóa`;

        buttonDeleteClicked = false;
        return;
    }
    buttonDeleteClicked = true;

});
$(document).ready(function () {
    $('.js-example-responsive').select2({
        tags: 'true',
        placeholder: "---Vui lòng chọn---",
        allowClear: true,
        width: 'resolve',

    });
});
$(document).ready(function () {
    $('.js-example-basic-single').select2();
});
