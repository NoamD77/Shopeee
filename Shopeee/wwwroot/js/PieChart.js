function PieChart(data, w, h) {
    var margin = {
        top: 20,
        right: 20,
        bottom: 50,
        left: 40
    },
        width = w - margin.left - margin.right,
        height = h - margin.top - margin.bottom,
        radius = Math.min(width, height) / 2;


    var color = d3.scale.ordinal()
        .range(["#98abc5", "#8a89a6", "#7b6888", "#6b486b", "#a05d56", "#d0743c", "#ff8c00", "#ff8c11","#aa77bb","#89a3bf"]);


    var arc = d3.svg.arc()
        .outerRadius(radius - 10)
        .innerRadius(radius - 70);

    var pie = d3.layout.pie()
        .sort(null)
        .value((d) => d.count);

    var svg = d3.select("svg#pieChart")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    var g = svg.selectAll(".arc")
        .data(pie(data))
        .enter().append("g")
        .attr("class", "arc");

    g.append("path")
        .attr("d", arc)
        .style("fill", (d) => color(d.data.product));

    g.append("text")
        .attr("transform", (d) => "translate(" + arc.centroid(d) + ")")
        .attr("dy", ".35em")
        .style("text-anchor", "middle")
        .text((d) => d.data.product + " (" + d.data.count + ")");
}