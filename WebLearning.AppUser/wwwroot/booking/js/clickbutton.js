function day() {
    var day = document.getElementById("day-container");
    var week = document.getElementById("week-container");
    var month = document.getElementById("month-container");
    var year = document.getElementById("year-container");

    if (day.style.display !== "none") {
        day.style.display = "block";
        week.style.display = "none";
        month.style.display = "none";
        year.style.display = "none";
    }
    else {
        day.style.display = "block";
        week.style.display = "none";
        month.style.display = "none";
        year.style.display = "none";

    }
};
function week() {
    var day = document.getElementById("day-container");
    var week = document.getElementById("week-container");
    var month = document.getElementById("month-container");
    var year = document.getElementById("year-container");

    if (week.style.display !== "none") {
        week.style.display = "block";
        day.style.display = "none";
        month.style.display = "none";
        year.style.display = "none";

    }
    else {
        week.style.display = "block";
        day.style.display = "none";
        month.style.display = "none";
        year.style.display = "none";

    }
};
function month() {
    var day = document.getElementById("day-container");
    var week = document.getElementById("week-container");
    var month = document.getElementById("month-container");
    var year = document.getElementById("year-container");

    if (month.style.display !== "none") {
        month.style.display = "block";
        day.style.display = "none";
        week.style.display = "none";
        year.style.display = "none";

    }
    else {
        month.style.display = "block";
        day.style.display = "none";
        week.style.display = "none";
        year.style.display = "none";

    }
};
function year() {
    var day = document.getElementById("day-container");
    var week = document.getElementById("week-container");
    var month = document.getElementById("month-container");
    var year = document.getElementById("year-container");

    if (year.style.display !== "none") {
        month.style.display = "none";
        day.style.display = "none";
        week.style.display = "none";
        year.style.display = "block";

    }
    else {
        year.style.display = "block";
        day.style.display = "none";
        week.style.display = "none";
        month.style.display = "none";

    }
};

