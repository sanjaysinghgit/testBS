$(document).ready(function () {
    $.getJSON("/MLM.Web/api/NewsModels/GetNewsModels",
   function (Data) {
       var Xmonth = new Array();
       Xmonth[0] = "Jan";
       Xmonth[1] = "Feb";
       Xmonth[2] = "Mar";
       Xmonth[3] = "Apr";
       Xmonth[4] = "May";
       Xmonth[5] = "June";
       Xmonth[7] = "Aug";
       Xmonth[8] = "Sept";
       Xmonth[9] = "Oct";
       Xmonth[10] = "Nov";
       Xmonth[11] = "Dec";
       Xmonth[6] = "July";
       Xmonth[7] = "Aug";
       Xmonth[8] = "Sept";
       Xmonth[9] = "Oct";
       Xmonth[10] = "Nov";
       Xmonth[11] = "Dec";

       console.log("Start News js");

       var str = "";
       $.each(Data, function (key, val) {


           var d = new Date(val.CreatedDate);
           // alert(d);
           var year = d.getFullYear();
           var month = Xmonth[d.getUTCMonth()];
           var day = d.getUTCDate();


           str = str + "<div class=\"post-row\">" +
            "<div class=\"left-meta-post\">" +
                "<div class=\"post-date\"><span class=\"day\">" + day + "</span><span class=\"month\">" + month + "</span></div>" +
                "<div class=\"post-type\"></div></div>" +
                "<h3 class=\"post-title\"><a href=\"#\">" + val.NewsTitle + "</a></h3>" + "<div class=\"post-content\">" +
                "<p>" + val.NewsDetails + " <a class=\"read-more\" href=\"/Home/BusinessPlan\">Read More...</a></p>" +
                "</div></div>";


       });
       document.getElementById("News").innerHTML = str;
       //alert(str);
   });
});