//This will generate a chart when the currencies changes

$(document).ready(function () {
    $("#ddlFromCurrency, #ddlToCurrency").change(function () {

        //Clean the div for a new chart
        $("#chart").empty();
        $("#legend").empty();
        $("#smoother").empty();

        var seriesData = [[], []];
        var fromCurrency = document.querySelector("#ddlFromCurrency").value;
        var toCurrency = document.querySelector("#ddlToCurrency").value;

        // Calls the controller to get the rates and unix time
        $.ajax({
            type: "POST",
            url: "/Exchange/GetChartData",
            contentType: "application/json; charset=utf-8",
            data: '{"fromCurrency":"' + fromCurrency + '","toCurrency":"' + toCurrency + '"}',
            dataType: "json",
            success: function (result) {
                result.forEach(function (element) {
                    if (element.Name == fromCurrency) {
                        seriesData[0].push({ x: element.Time, y: element.Rate });
                    }
                    else {
                        seriesData[1].push({ x: element.Time, y: element.Rate });
                    }
                });
  
                //instantiate a new chart with the data
                var graph = new Rickshaw.Graph({
                    element: document.getElementById("chart"),
                    width: 960,
                    height: 500,
                    renderer: 'line',
                    series: [
                        {
                            color: "black",
                            data: seriesData[0],
                            name: fromCurrency
                        }, {
                            color: "#148f4f",
                            data: seriesData[1],
                            name: toCurrency
                        }
                    ]
                });

                graph.render();

                var hoverDetail = new Rickshaw.Graph.HoverDetail({
                    graph: graph
                });

                var legend = new Rickshaw.Graph.Legend({
                    graph: graph,
                    element: document.getElementById('legend')

                });

                var shelving = new Rickshaw.Graph.Behavior.Series.Toggle({
                    graph: graph,
                    legend: legend
                });

                var axes = new Rickshaw.Graph.Axis.Time({
                    graph: graph
                });
                axes.render();

                document.querySelector("#chart_container").style.visibility = 'visible';

            }


        });
        
    });
});
