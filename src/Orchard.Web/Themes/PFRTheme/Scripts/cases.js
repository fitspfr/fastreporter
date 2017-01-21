var yearsToDisplay = 5;
var currentDisplayStartYear = 2013;
var currentDisplayEndYear = 2017;
var yearDisplayDiv = 'divYearNav';
/*
Existing site logic
var first = 0; var last = 2;
var yeararray = new Array('<a href="/pfr/2010" style="text-decoration:none;color:Gray;">2010</a>&nbsp;<a href="/pfr/2011" style="text-decoration:none;color:Gray;">2011</a>&nbsp;<a href="/pfr/2013" style="text-decoration:none;color:Gray;">2013</a>&nbsp;<a href="/pfr/2014" style="text-decoration:none;color:Gray;">2014</a>&nbsp;<a href="/pfr/2015" style="text-decoration:none;color:Gray;">2015</a>&nbsp;', '<a href="/pfr/1998" style="text-decoration:none;color:Gray;">1998</a>&nbsp;<a href="/pfr/2006" style="text-decoration:none;color:Gray;">2006</a>&nbsp;<a href="/pfr/2007" style="text-decoration:none;color:Gray;">2007</a>&nbsp;<a href="/pfr/2008" style="text-decoration:none;color:Gray;">2008</a>&nbsp;<a href="/pfr/2009" style="text-decoration:none;color:Gray;">2009</a>&nbsp;', '<a href="/pfr/11" style="text-decoration:none;color:Gray;">11</a>&nbsp;<a href="/pfr/1234" style="text-decoration:none;color:Gray;">1234</a>&nbsp;<a href="/pfr/1345" style="text-decoration:none;color:Gray;">1345</a>&nbsp;<a href="/pfr/1980" style="text-decoration:none;color:Gray;">1980</a>&nbsp;');
function ProcessPrevious() { if (first == 0) { first = last; } else { first--; } document.getElementById('ctl01_mainContent_ctl00_lbl').innerHTML = yeararray[first]; }
function ProcessNext() { if (first == last) { first = 0; } else { first++; } document.getElementById('ctl01_mainContent_ctl00_lbl').innerHTML = yeararray[first]; }
*/

function setYears(yearDiff) {
    currentDisplayStartYear += yearDiff;
    currentDisplayEndYear += yearDiff;
    setYearsDisplay(currentDisplayStartYear);
}
function setYearsDisplay(startYear, divName) {
    var currentYear = new Date().getFullYear();
    var yearCount = 0;
    var navBarHtml = "";
    if (divName != null && divName != undefined && divName != '') yearDisplayDiv = divName;
    navBarHtml += '<input id="lftBtn" value="<" onclick="setYears(-2);" style="background-color:White;border-color:White;border-style:none;color:Gray;padding:0px 5px 0px 0px;margin:5px;" type="button"/>';
    //navBarHtml += '<a href="javascript:void(0);" onclick="setYears(-2);" class="leftNav">&lt;</a>';
    for (var i = startYear; i <= currentYear; i++) {
        if (yearCount >= yearsToDisplay)
            break;
        currentDisplayEndYear = i;
        navBarHtml += '<a href="./pfr-filter?year='+ i + '" style="text-decoration:none;color:Gray;">' + i + '</a>&nbsp;';
        yearCount++;
    }
    navBarHtml += '<input id="RightBtn" value=">" onclick="setYears(2);" style="background-color:White;border-color:White;border-style:none;color:Gray;padding:0px;margin:5px;" type="button"/>';
    //navBarHtml += '<a href="javascript:void(0);" onclick="setYears(2);" class="rightNav">&gt;</a>';
    //$("#divYearNav").html(navBarHtml);
    $("#" + yearDisplayDiv).html(navBarHtml);
}
function showYear(year) {
    //alert(year);
}

function printDiv(divName) {
    var printContents = document.getElementById(divName).innerHTML;
    w = window.open();

    w.document.write(printContents);
    w.document.write('<scr' + 'ipt type="text/javascript">' + 'window.onload = function() { window.print(); window.close(); };' + '</sc' + 'ript>');

    w.document.close(); // necessary for IE >= 10
    w.focus(); // necessary for IE >= 10

    return;
}