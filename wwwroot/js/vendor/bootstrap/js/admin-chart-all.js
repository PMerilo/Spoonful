﻿// Chart.js scripts
// -- Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';
// -- Area Chart Example

var getRevenueData = $.get('/Admin/Invoices')
getRevenueData.done(function (data) {
    var ctxrevenue = document.getElementById("myAreaChart");
   
    labels = [];

    let values = []
    data.forEach(element => {
        labels.push(element.dateOfPayment)

    });


    data.forEach(element => {
        values.push(element.cost)

    });
    var myLineChart = new Chart(ctxrevenue, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: "Sessions",
                lineTension: 0.3,
                backgroundColor: "rgba(2,117,216,0.2)",
                borderColor: "rgba(2,117,216,1)",
                pointRadius: 5,
                pointBackgroundColor: "rgba(2,117,216,1)",
                pointBorderColor: "rgba(255,255,255,0.8)",
                pointHoverRadius: 5,
                pointHoverBackgroundColor: "rgba(2,117,216,1)",
                pointHitRadius: 20,
                pointBorderWidth: 2,
                data: data,
            }],
        },
        options: {
            scales: {
                xAxes: [{
                    time: {
                        unit: 'date'
                    },
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        maxTicksLimit: 7
                    }
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: 40000,
                        maxTicksLimit: 5
                    },
                    gridLines: {
                        color: "rgba(0, 0, 0, .125)",
                    }
                }],
            },
            legend: {
                display: false
            }
        }
    });
})
// -- Bar Chart Example
var getusedVouchersData = $.get('/Admin/charts/usedVouchers')
getusedVouchersData.done(function (data) {
    var ctx = document.getElementById("myBarChart");
    labels = [];
    let values = []

    data.forEach(element => {
        labels.push(element.usedVouchers)

    });

    data.forEach(element => {
        values.push(element.used)

    });
    var myLineChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: "Used Vouchers",
                backgroundColor: "rgba(2,117,216,1)",
                borderColor: "rgba(2,117,216,1)",
                data: values,
            }],
        },
        options: {
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        maxTicksLimit: 6
                    }
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: 10,
                        maxTicksLimit: 5
                    },
                    gridLines: {
                        display: true
                    }
                }],
            },
            legend: {
                display: false
            }
        }
    });
})
// -- Pie Chart Example
var getMenuPreferenceData = $.get('/Admin/charts')
getMenuPreferenceData.done(function (data) {
    var ctx = document.getElementById("myPieChart");
    labels = [];
    let values = []

    data.forEach(element => {
        labels.push(element.menuPreference)

    });

    data.forEach(element => {
        values.push(element.count)

    });

    
    var myPieChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: ['#007bff', '#dc3545', '#ffc107', '#28a745'],
            }],
        },
    });

  
})