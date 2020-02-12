//This is the OnLoad event of the page, it populates the dropdown lists with currencies
$(window).on("load",function () {                  
    $.get("/Exchange/GetCurrencies", null, function (data) {

            // Use jQuery's each to iterate over the response 
            $.each(data, function (i, Currencies) {
                // appends the currencies for the dropdown lists
                $('#ddlFromCurrency').append('<option value="' + Currencies  + '">' + Currencies + '</option>');
                $('#ddlToCurrency').append('<option value="' + Currencies + '">' + Currencies + '</option>');
            });        
        });         
});  

//This is the OnChange event on the Amount input, it will call the Controller to execute the exchange
$(document).ready(function () {
    $("#btnConvert").click(function () {
        $.ajax({
            type: "POST",
            url: "/Exchange/CalculateExchange",
            contentType: "application/json; charset=utf-8",
            data: '{"fromCurrency":"' + $("#ddlFromCurrency").val() + '","ToCurrency":"' + $("#ddlToCurrency").val() + '","value":"' + $("#txtAmount").val() +  '"}',
            dataType: "html",
            success: function (result, status, xhr) {
                $("#lblValue").text(result);
                
            }
        });
    });
});


//This is the OnClick event on the swap button
$(document).ready(function () {
    $("#btnSwap").click(function () {

        var FromValue = $("#ddlFromCurrency").val();
        var ToValue = $("#ddlToCurrency").val();

        $("#ddlFromCurrency").val(ToValue);
        $("select#ddlToCurrency option:checked").val(FromValue);
    });
});

