// ------------------Chi tiết tài sản JS-----------------------
var submitUpdateButton = document.getElementById('updatebutton');
var submitDeleteButton = document.getElementById('deletebutton');
//var submitSeeMoveAssetDetailButton = document.getElementById('seemovedetailbutton');
var submitSeeMoveAssetDetailButton = document.getElementById('seemovedetailbutton');

let buttonUpdateClicked = false;
let buttonDeleteClicked = false;
//let buttonMoveAssetDetailClicked = false;
let buttonMoveAssetDetailClicked = false;
submitSeeMoveAssetDetailButton.addEventListener('click', function handleClick() {
    $(".moveassetContainer").css("display", "block");
    submitSeeMoveAssetDetailButton.textContent = 'Hủy thao tác xem';


    if (buttonMoveAssetDetailClicked) {
        $(".moveassetContainer").css("display", "none");
        submitSeeMoveAssetDetailButton.innerHTML = `<i class="fa fa-history"></i> Lịch sử điều chuyển `;

        buttonMoveAssetDetailClicked = false;
        return;
    }
    buttonMoveAssetDetailClicked = true;

});
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
