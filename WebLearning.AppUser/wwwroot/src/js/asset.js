
var cbstate = JSON.parse(localStorage['CBState'] || '{}');

var checkBoxItem = document.getElementsByClassName('checkBoxTable');

var historyCheck = [];
//$('input[type="checkbox"][name="assetsCategoryId"]').on('change', function () {
//    $('#formGetData').submit();

//});
//$('input[type="checkbox"][name="assetsSubCategoryId"]').on('change', function () {
//    $('#formGetData').submit();
//});
//$('input[type="checkbox"][name="assetsDepartmentId"]').on('change', function () {
//    $('#formGetData').submit();
//});
//$('input[type="checkbox"][name="assetsStatusId"]').on('change', function () {
//    $('#formGetData').submit();
//});
//$('input[type="checkbox"][name="assetsDepartmentId"]').on('change', function () {
//    $('#formGetData').submit();
//});
//$('input[type="checkbox"][name="assetsDepartmentLocationId"]').on('change', function () {
//    $.ajax({
//        url: '/Asset/GetDepartmentWithLocation',
//        datatype: "json",
//        type: "POST",
//        data: { parentCode: $(this).val()},
//        success: function (results) {
//            $("#checkboxdepfilter").html("");
//            $('#checkboxdepfilter').html(results);
//        },

//    })
//});
//var selectedDep;
//$('input[type="checkbox"][name="assetsDepartmentLocationId"]').on('change', function () {
//    selectedDep = $('input[type="checkbox"][name="assetsDepartmentLocationId"]:checked').map(function () {

//        return $(this).val();
//    }).get();
//    $.ajax({
//        url: '/Asset/GetDepartmentWithLocation',
//        datatype: "json",
//        type: "POST",
//        data: { parentCode: selectedDep },
//        success: function (results) {
//            $("#checkboxdepfilter").html("");
//            $('#checkboxdepfilter').html(results);
//        },

//    })
//});


$(function () {
    
    $("#removeCheckedBox").on("click", function () {
        localStorage.removeItem("CBState");

        location.href = "tai-san.html"
    });
    $("#deleteAsset").on("click", function () {
        localStorage.removeItem("CBState");

    });
    $("#assetsCategoryCheckAll").click(function () {
        $('input[type="checkbox"][name="assetsCategoryId"]').not(this).prop('checked', this.checked);
        var checkboxCat = document.getElementById("checkboxcatfilter").getElementsByClassName("checkBoxFilter")
        for (var i = 0; i < checkboxCat.length; i++) {

            if ($(checkboxCat[i]).prop("checked") == true) {

                cbstate[$(checkboxCat[i]).attr("data-mahh")] = $(checkboxCat[i]).attr("data-tencb");

            }
            else if (cbstate[$(checkboxCat[i]).attr("data-mahh")]) {

                delete cbstate[$(checkboxCat[i]).attr("data-mahh")];

            }
            localStorage.CBState = JSON.stringify(cbstate);

        }
        


    });
    $("#assetsDepLocationCheckAll").click(function () {
        $('input[type="checkbox"][name="assetsDepartmentLocationId"]').not(this).prop('checked', this.checked);
        var checkboxCat = document.getElementById("checkboxdeplocationfilter").getElementsByClassName("checkBoxFilter")
        for (var i = 0; i < checkboxCat.length; i++) {

            if ($(checkboxCat[i]).prop("checked") == true) {

                cbstate[$(checkboxCat[i]).attr("data-mahh")] = $(checkboxCat[i]).attr("data-tencb");

            }
            else if (cbstate[$(checkboxCat[i]).attr("data-mahh")]) {

                delete cbstate[$(checkboxCat[i]).attr("data-mahh")];

            }
            localStorage.CBState = JSON.stringify(cbstate);

        }
        


    });
    $("#assetsDepCheckAll").click(function () {
        $('input[type="checkbox"][name="assetsDepartmentId"]').not(this).prop('checked', this.checked);
        var checkboxCat = document.getElementById("checkboxdepfilter").getElementsByClassName("checkBoxFilter")
        for (var i = 0; i < checkboxCat.length; i++) {

            if ($(checkboxCat[i]).prop("checked") == true) {

                cbstate[$(checkboxCat[i]).attr("data-mahh")] = $(checkboxCat[i]).attr("data-tencb");

            }
            else if (cbstate[$(checkboxCat[i]).attr("data-mahh")]) {

                delete cbstate[$(checkboxCat[i]).attr("data-mahh")];

            }
            localStorage.CBState = JSON.stringify(cbstate);

        }
        

    });
    $("#assetsStatusCheckAll").click(function () {
        $('input[type="checkbox"][name="assetsStatusId"]').not(this).prop('checked', this.checked);
        var checkboxCat = document.getElementById("checkboxstatusfilter").getElementsByClassName("checkBoxFilter")
        for (var i = 0; i < checkboxCat.length; i++) {

            if ($(checkboxCat[i]).prop("checked") == true) {

                cbstate[$(checkboxCat[i]).attr("data-mahh")] = $(checkboxCat[i]).attr("data-tencb");

            }
            else if (cbstate[$(checkboxCat[i]).attr("data-mahh")]) {

                delete cbstate[$(checkboxCat[i]).attr("data-mahh")];

            }
            localStorage.CBState = JSON.stringify(cbstate);

        }
        


    });
    window.addEventListener('load', function () {
        // Get the current state from localstorage
        // State is stored as a JSON string
        // Loop through state array and restore checked
        // state for matching elements
        for (var i in cbstate) {
            var el = document.querySelector('input[data-mahh="' + i + '"]');
            if (el) el.checked = true;
            if (i.includes("IT") == false && i.includes("CCA") == false && i.includes("DLCA") == false && i.includes("DCA") == false && i.includes("STTCA") == false) {
                var option = "<span id='historyCheckBox_" + i + "' style='-webkit-border-radius: 2px;display: block;font-size:14px;float: left;padding: 2px 6px;background: #1ABB9C;color: #F1F6F7;margin-right: 5px;font-weight: 500;margin-bottom: 5px;font-family: helvetica;cursor:pointer' title='Mã_" + i + "'><span class='historyCheckBoxContent'>" + cbstate[i] + "&nbsp;&nbsp;</span><a style='cursor:pointer;color:white' onclick=" + "RemoveSession('" + i + "')" + "" + " >x</a></span>";
                $('.historyCheckBoxContainer').append(option);
            }

        }

        // Get all checkboxes that you want to monitor state for
        var cb = document.getElementsByClassName('checkBoxFilter');

        var cbTb = document.getElementsByName('table_records');


        // Loop through results and ...
        for (var i = 0; i < cb.length; i++) {
            var rowConItem = document.getElementById("rowContainer-" + $(this).attr("data-mahh"));

            //bind click event handler
            cb[i].addEventListener('click', function (evt) {
                // If checkboxe is checked then save to state
                if (this.checked) {
                    cbstate[$(this).attr("data-mahh")] = $(this).attr("data-tencb");

                }

                // Else remove from state
                else if (cbstate[$(this).attr("data-mahh")]) {
                    delete cbstate[$(this).attr("data-mahh")];

                    $(rowConItem).css({ "background-color": "white" });

                }

                // Persist state
                localStorage.CBState = JSON.stringify(cbstate);
            });
        }
        for (var i = 0; i < cbTb.length; i++) {
            cbTb[i].addEventListener('click', function () {
                var rowConItem = document.getElementById("rowContainer-" + $(this).attr("data-mahh"));

                var buttonConItem = document.getElementById("buttonTable-" + $(this).attr("data-mahh"));

                if (this.checked) {
                    cbstate[$(this).attr("data-mahh")] = $(this).attr("data-tencb");

                    $(rowConItem).css({ "background-color": "rgba(38,185,154,.16)" });
                    $(buttonConItem).css({ "display": "none" });

                }
                // Else remove from state
                else if (cbstate[$(this).attr("data-mahh")]) {
                    delete cbstate[$(this).attr("data-mahh")];

                    $(rowConItem).css({ "background-color": "white" });
                    $(buttonConItem).css({ "display": "block" });
                }

                // Persist state
                localStorage.CBState = JSON.stringify(cbstate);

            });

        }
    });
});


 function checkUncheck(checkBox) {

    var checkBoxItem = document.getElementsByClassName('checkBoxTable');

    var buttonsItem = document.getElementsByClassName("buttonTable");

     var checkAll = document.getElementById("check-all");

     var disableItems = document.getElementById("disableItems");

    for (var i = 0; i < buttonsItem.length; i++) {
        if (checkAll.checked) {
            $(".buttonTable").css("display", "none");

            $(".rowContainer").css({ "background-color": "rgba(38,185,154,.16)" });

        } else {
            $(buttonsItem[i]).css("display", "block");

            $(".rowContainer").css("background-color", "white");

        }
    };

     

    for (var i = 0; i < checkBoxItem.length; i++) {

        checkBoxItem[i].checked = checkBox.checked;

        if ($(checkBoxItem[i]).prop("checked") == true) {

            cbstate[$(checkBoxItem[i]).attr("data-mahh")] = $(checkBoxItem[i]).attr("data-tencb");

        }
        else if (cbstate[$(checkBoxItem[i]).attr("data-mahh")]) {

            delete cbstate[$(checkBoxItem[i]).attr("data-mahh")];

        }
        localStorage.CBState = JSON.stringify(cbstate);

     }
     
}
for (var i in cbstate) {

    var rowItem = document.getElementById("rowContainer-" + i);

    var buttonConItem = document.getElementById("buttonTable-" + i);

    $(rowItem).css({ "background-color": "rgba(38,185,154,.16)" });

    $(buttonConItem).css({ "display": "none" });
    
}

function RemoveSession(i) {
    delete cbstate[i];
    localStorage.CBState = JSON.stringify(cbstate);
    $('input[data-mahh="' + i + '"]').prop("checked", false);
    
    ($("#historyCheckBox_" + i)).remove();

    //$('#formGetData').submit();

}
//$.each(cbstate, function (i, item) {
//    var option = "<span style='-webkit-border-radius: 2px;display: block;float: left;padding: 2px 6px;background: #1ABB9C;color: #F1F6F7;margin-right: 5px;font-weight: 500;margin-bottom: 5px;font-family: helvetica; '><span>" + i + "&nbsp;&nbsp;</span><a style='cursor:pointer' onclick=" + "RemoveSession('" + i + "')" + " title='Removing tag'>x</a></span>";
//    $('.historyCheckBoxContainer').append(option);
//});
function deleteAllData() {
    var record = [];
    for (var i in cbstate) {

        if (i.includes("IT")) {

            record.push(i);

        }
    }
    $('[name=table_records]').val(record);
    $.ajax({
        url: '/Asset/GetItem',
        datatype: "json",
        type: "POST",
        data: { id: record },
        success: function (results) {
            $("#sid").html("");
            $("#sid").html(results);
        },

    })
}
function updateRepairItem() {
    var record = [];
    for (var i in cbstate) {

        if (i.includes("IT")) {

            record.push(i);

        }
    }
    $('[name=table_records]').val(record);
    
    $.ajax({
        url: '/Asset/GetItem',
        datatype: "json",
        type: "POST",
        data: { id: record },
        success: function (results) {
            $("#siur").html("");
            $("#siur").html(results);
        },
        
    })
}
function exportAll() {
    var active = false;
    if ($('input[type = "checkbox"][name = "active"]').prop("checked") == true) {
        active = true;
    }

    var address = location.search;

    if (address.length <= 0) address += "?active=true";
    else {
        if(active == true){
            address = address.replace("active=true", "active=false");
        }
        else {
            address += "&active=true";

        }
    }

    $('#formExport').append(`<input type="text" id="querySearch" value=${address}  name="querySearch"  style='display:none' />`);

    //$.ajax({
    //    url: "/Asset/Export",
    //    type: "get",
    //    data:
    //    {
    //        address:address


    //    },
    //});
}
function loadDataMoveAll() {
    var record = [];
    for (var i in cbstate) {

        if (i.includes("IT")) {

            record.push(i);

        }
    }
    $('[name=table_records]').val(record);
    $.ajax({
        url: '/Asset/GetItemViewSelected',
        datatype: "json",
        type: "POST",
        data: { id: record },
        success: function (results) {
            $("#si").html("");
            $("#si").html(results);
        }

    })
}
function checkMove() {
    $.ajax({
        url: '/Asset/UpdateItemViewSelected',
        datatype: "json",
        type: "POST",
        data: { id: $('[id=moveId]').val() },
        success: function (results) {
            $("#si").html("");
            $("#si").html(results);
        }

    })
}

function deleteAllData() {
    var record = [];
    for (var i in cbstate) {

        if (i.includes("IT")) {

            record.push(i);

        }
    }
    $('[name=table_records]').val(record);
    $.ajax({
        url: '/Asset/GetItemViewSelected',
        datatype: "json",
        type: "POST",
        data: { id: record },
        success: function (results) {
            $("#sid").html("");
            $("#sid").html(results);
        }

    })
}
function checkDelete() {
    $.ajax({
        url: '/Asset/UpdateItemViewSelected',
        datatype: "json",
        type: "POST",
        data: { id: $('[id=deleteId]').val() },
        success: function (results) {
            $("#sid").html("");
            $("#sid").html(results);
        }

    })
}
function historyMoved() {
    $.ajax({
        url: '/lich-su-dieu-chuyen.html',
        datatype: "json",
        type: "GET",
        success: function (results) {
            $("#resulthm").html("");
            $("#resulthm").html(results);
        }

    })
}