try {
    navigator.geolocation.getCurrentPosition(success);
    
}
catch (Exception) {
    console.log("BU");
}

function success(position) {
    const latitude = position.coords.latitude;
    const longtitude = position.coords.longitude;

    var map = L.map('mapid').setView([latitude, longtitude], 13);

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=sk.eyJ1Ijoic2Fra2FnZSIsImEiOiJja2kxcXFzeG8wNTVxMzNudGNmcG16a2YxIn0.J8SFpo3K4nnmAKnFwf61Sg', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'sk.eyJ1Ijoic2Fra2FnZSIsImEiOiJja2kxcXFzeG8wNTVxMzNudGNmcG16a2YxIn0.J8SFpo3K4nnmAKnFwf61Sg',
        marker: marker
    }).addTo(map);

    var marker = L.marker([latitude, longtitude], {
    }).addTo(map).bindTooltip(userTooltip);

    var dataCoords = { latitude: latitude, longtitude: longtitude };

    $(document).ready(function (){
        $.ajax({
            url: "SaveCoordsToDb",
            data: JSON.stringify(dataCoords),
            dataType: "json",
            type: "POST",
            contentType: 'application/json; charset=utf-8'

        });
    });

    

   /* $.post('/Map/SaveCoordsToDb', { dataa }), function (data) {
        alert("Success " + data.success);
    };*/
}









 



