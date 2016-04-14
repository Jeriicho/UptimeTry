

$(document).ready(function () {
    var minuApi = '5b4abdd11e9f4f5b86b276d974e174ad'
    var url = 'https://openexchangerates.org/api/latest.json?app_id=' + minuApi;
    var suhe =$.ajax({
        url: url,
        datatype: "json",
        async: false,
    }).responseJSON; // ootab AJAXi requesti ära ning asetab tagastatud JSONi muutujasse 'suhe'


    
    //hinnamuutus
    $("#EUR").click(function () {
        if (document.getElementsByClassName("valuuta")[0].innerHTML === "£") {
            for (i in document.getElementsByClassName("valuuta")) {
                document.getElementsByClassName("valuuta")[i].innerHTML = "€";
                document.getElementsByClassName("hind")[i].innerHTML = Number(((parseFloat(document.getElementsByClassName("hind")[i].innerHTML) / suhe.rates.GBP) * suhe.rates.EUR).toFixed(2)).toString();
            }
        }

        if (document.getElementsByClassName("valuuta")[0].innerHTML === "$") {
            for (i in document.getElementsByClassName("valuuta")) {
                document.getElementsByClassName("valuuta")[i].innerHTML = "€";
                document.getElementsByClassName("hind")[i].innerHTML = Number((parseFloat(document.getElementsByClassName("hind")[i].innerHTML) / suhe.rates.EUR).toFixed(2)).toString();
            }
        }
    });
    $("#GBP").click(function () {
        if (document.getElementsByClassName("valuuta")[0].innerHTML === "$") {
            for (i in document.getElementsByClassName("valuuta")) {
                document.getElementsByClassName("valuuta")[i].innerHTML = "£";
                document.getElementsByClassName("hind")[i].innerHTML = Number((parseFloat(document.getElementsByClassName("hind")[i].innerHTML) / suhe.rates.GBP).toFixed(2)).toString();
            }
        }

        if (document.getElementsByClassName("valuuta")[0].innerHTML === "€") {
            for (i in document.getElementsByClassName("valuuta")) {
                document.getElementsByClassName("valuuta")[i].innerHTML = "£";
                document.getElementsByClassName("hind")[i].innerHTML = Number(((parseFloat(document.getElementsByClassName("hind")[i].innerHTML) / suhe.rates.EUR) * suhe.rates.GBP).toFixed(2)).toString();
            }
        }
    });

    $("#USD").click(function () {
        if (document.getElementsByClassName("valuuta")[0].innerHTML === "€") {
            for (i in document.getElementsByClassName("valuuta")) {
                document.getElementsByClassName("valuuta")[i].innerHTML = "$";
                document.getElementsByClassName("hind")[i].innerHTML = Number((parseFloat(document.getElementsByClassName("hind")[i].innerHTML) * suhe.rates.EUR).toFixed(2)).toString();
            }
        }

        if (document.getElementsByClassName("valuuta")[0].innerHTML === "£") {
            for (i in document.getElementsByClassName("valuuta")) {
                document.getElementsByClassName("valuuta")[i].innerHTML = "$";
                document.getElementsByClassName("hind")[i].innerHTML = Number((parseFloat(document.getElementsByClassName("hind")[i].innerHTML) * suhe.rates.GBP).toFixed(2)).toString();
            }
        }
    });
});

