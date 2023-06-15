var values = [];

var cbstate = JSON.parse(localStorage['CBState'] || '{}');

var checkBoxItem = document.getElementsByClassName('checkBoxTable');

$(function () {
    //$('input[type="checkbox"][name="assetsCategoryId"]').on('change', function () {
    //    $('#form').submit();
    //});
    //$('input[type="checkbox"][name="assetsSubCategoryId"]').on('change', function () {
    //    $('#form').submit();
    //});
    //$('input[type="checkbox"][name="assetsDepartmentId"]').on('change', function () {
    //    $('#form').submit();
    //});
    //$('input[type="checkbox"][name="assetsStatusId"]').on('change', function () {
    //    $('#form').submit();
    //});
    $("#removeCheckedBox").on("click", function () {
        localStorage.removeItem("CBState");
        window.location.href = "/tai-san.html"
    });
    // variable to store our current state
    // bind to the onload event
    window.addEventListener('load', function () {

        // Get the current state from localstorage
        // State is stored as a JSON string
        // Loop through state array and restore checked
        // state for matching elements
        for (var i in cbstate) {
            var el = document.querySelector('input[data-mahh="' + i + '"]');
            if (el) el.checked = true;
        }

        // Get all checkboxes that you want to monitor state for
        var cb = document.getElementsByClassName('checkBoxFilter');

        var cbTb = document.getElementsByName('table_records');


        // Loop through results and ...
        for (var i = 0; i < cb.length; i++) {

            //bind click event handler
            cb[i].addEventListener('click', function (evt) {
                // If checkboxe is checked then save to state
                if (this.checked) {
                    cbstate[$(this).attr("data-mahh")] = true;

                }

                // Else remove from state
                else if (cbstate[$(this).attr("data-mahh")]) {
                    delete cbstate[$(this).attr("data-mahh")];
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
                    cbstate[$(this).attr("data-mahh")] = true;

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

            //rowChecked++;

            cbstate[$(checkBoxItem[i]).attr("data-mahh")] = true;

            values.push($(checkBoxItem[i]).attr("data-mahh"));


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
function moveAll() {
    var record = [];

    for (var i in cbstate) {

        if (i.includes("IT")) {

            record.push(i);

        }
    }
    $.ajax({
        url: "/Asset/MoveAsset",
        type: "POST",
        data: { table_records: record },
        success: function (response) {
            alert(response);
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }

    });
}
function deleteAll() {
    var record = [];
    for (var i in cbstate) {
        if (i.includes("IT")) {
            record.push(i);

        }
    }
    $.ajax({
        url: "/Asset/DeleteAsset",
        type: "POST",
        data: { table_records: record },
        success: function (response) {
            alert(response);
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }

    });
}
