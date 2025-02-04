// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function AddLikeToArticle(articleId, userId) {
    $.ajax({
        type: 'post',
        url: '/Article/AddLikeToArticle',
        dataType: 'json',
        data: {
            articleId: articleId,
            userId: userId
        },
        success: function (count) {
            onLikeSuccess(count);
        }
    })
}

function RemoveLikeFromArticle(articleId, userId) {
    $.ajax({
        type: 'post',
        url: '/Article/RemoveLikeFromArticle',
        dataType: 'json',
        data: {
            articleId: articleId,
            userId: userId
        },
        success: function () {
            onLikeSuccess();
        }
    })
}

// Reload Div with id cardFooter
function onLikeSuccess() {
    $('#cardFooter').load(window.location.href + " #cardFooter");
}

function AddViewToArticle(articleId) {
    $.ajax({
        type: 'post',
        url: '/Article/AddViewToArticle',
        dataType: 'json',
        data: {
            articleId: articleId
        }
    })
}

function GetSummary(articleId) {
    $.ajax({
        type: 'post',
        url: '/Article/GetSummary',
        dataType: 'json',
        data: {
            articleId: articleId
        },
        success: function (summary) {
            $("#summaryButton").removeClass("spinner-border spinner-border-sm")
            $('#summary').html(summary)
        }
    })
}

function Spinner() {
    $("#summaryButton").addClass("spinner-border spinner-border-sm");
}

function getMonthName(monthNumber) {
    const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    if (monthNumber >= 1 && monthNumber <= 12) {
        return monthNames[monthNumber - 1];
    } else {
        return "Invalid month number";
    }
}


function ChartStat() {
    $.ajax({
        url: '/Admin/SubscriptionStatistics',
        type: 'post',
        dataType: 'json',
        success: function (data) {
            console.log('R');
            // map the data to labels and data arrays 2024
            const labels24 = data.l24.map(x => getMonthName(x.month));
            console.log(labels24);
            const newSub24 = data.l24.map(x => x.newSubscriberCount);
            const allSub24 = data.l24.map(x => x.allSubscriberCount);

            // map the data to labels and data arrays 2025
            const labels25 = data.l25.map(x => getMonthName(x.month));
            console.log(labels25);
            const newSub25 = data.l25.map(x => x.newSubscriberCount);
            const allSub25 = data.l25.map(x => x.allSubscriberCount);

            //create the charts 2024
            const mns24 = $('#monthlyNewSub24');
            new Chart(mns24, {
                type: 'bar',
                data: {
                    labels: labels24,
                    datasets: [{
                        label: '2024',
                        data: newSub24,
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });

            const mas24 = $('#monthlyAllSub24');
            new Chart(mas24, {
                type: 'bar',
                data: {
                    labels: labels24,
                    datasets: [{
                        label: '2024',
                        data: allSub24,
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });

            //create the charts 2025
            const mns25 = $('#monthlyNewSub25');
            new Chart(mns25, {
                type: 'bar',
                data: {
                    labels: labels25,
                    datasets: [{
                        label: '2025',
                        data: newSub25,
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });

            const mas25 = $('#monthlyAllSub25');
            new Chart(mas25, {
                type: 'bar',
                data: {
                    labels: labels25,
                    datasets: [{
                        label: '2025',
                        data: allSub25,
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });

        },
    });
}

function ChartElPrice() {
    const today = new Date().toISOString().split('T')[0];
    $.ajax({
        url: '/Home/GetElPriceTodayData',
        type: 'post',
        dataType: 'json',
        success: function (data) {
            console.log(data);
            const labels = data.hours;
            const se1 = data.priceSekSE1;
            console.log(se1);
            const se2 = data.priceSekSE2;
            const se3 = data.priceSekSE3;
            const se4 = data.priceSekSE4;

            //create the charts
            const elChart = $('#elChart');
            new Chart(elChart, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'SE1',
                            data: se1,
                            backgroundColor: 'rgba(75, 100, 192, 0.2)',
                            borderColor: 'rgba(75, 192, 192, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'SE2',
                            data: se2,
                            backgroundColor: 'rgba(75, 1, 250, 0.2)',
                            borderColor: 'rgba(75, 192, 192, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'SE3',
                            data: se3,
                            backgroundColor: 'rgba(75, 192, 22, 0.2)',
                            borderColor: 'rgba(75, 192, 192, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'SE4',
                            data: se4,
                            backgroundColor: 'rgba(7, 12, 192, 222)',
                            borderColor: 'rgba(75, 192, 192, 1)',
                            borderWidth: 1
                        },                       
                        
                    ]
                },
                options: {
                    



                    plugins: {
                        //title: {
                        //    display: true,
                        //    text: today+': 24h El price (öre/h)',
                        //},
                        legend: {
                            position: 'chartArea',
                            align: 'start',
                            //maxWidth: 100,
                            //maxHeigth: 100,
                            labels: {
                                boxWidth: 10,
                                
                            },
                        },
                    },
                    scales: {
                        y: {
                            beginAtZero: true
                        },
                        
                    }
                }
            });
        },
    });

}