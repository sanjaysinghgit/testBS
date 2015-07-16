//<script src="../../Scripts/jquery-1.7.1.min.js"
//type="text/javascript"></script>

    $(document).ready(function () 
    {
        console.log("start Js--------------");
        $.getJSON("/MLM.web/api/TopAchivars/GetTopAchivars/",
       function (Data) {
 
           $.each(Data, function (key, val)
           {
               var str = val.name + '-City:' + val.location;
               $('<li>', { text: str })
               .appendTo($('#Topachivers'));
           });
       });
    });
 
//function show() {
//    var Id = $('#itId').val();
//    $.getJSON("api/items/" + Id,
//        function (Data) {
//            var str = Data.name + ': $' + Data.cost;
//            $('#items').text(str);
//        })
//    .fail(
//        function (jqXHR, textStatus, err) {
//            $('#items').text('Error: ' + err);
//        });
//}
