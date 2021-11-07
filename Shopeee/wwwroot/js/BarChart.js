function BarChart(data, w, h) {
    var margin = {
        top: 20,
        right: 20,
        bottom: 50,
        left: 40
    },
        width = w - margin.left - margin.right,
        height = h - margin.top - margin.bottom;

    console.log(data);
    var x = d3.scale.ordinal()
        .rangeRoundBands([width, 0], 0.1);

    var y = d3.scale.linear()
        .range([0, height]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .tickFormat(d3.format("d"))
        .tickSubdivide(0);

    var svg = d3.select("svg#barChart")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    x.domain(data.map((d) => d.product));

    y.domain([d3.max(data, (d) => d.count), 0]);

    svg.append("g")
        .attr("class", "y axis")
        .attr("transform", "translate(0," + height + ")")
        .call(xAxis)
        .selectAll("text")
        .attr("transform", "rotate(90)")
        .attr("x", 0)
        .attr("y", -6)
        .attr("dx", ".6em")
        .style("text-anchor", "start");

    svg.append("g")
        .attr("class", "y axis")
        .call(yAxis);

    svg.selectAll(".bar")
        .data(data)
        .enter()
        .append("rect")
        .attr("class", "bar")
        .attr("x", (d) => x(d.product))
        .attr("width", x.rangeBand())
        .attr("y", (d) => y(d.count))
        .attr("height", (d) => height - y(d.count));
}