function updatePersonCount(value) {
    document.getElementById('personCount').innerText = value;
}


function validateForm() {
    var pickupLat = document.getElementById('pickupLat').value;
    var pickupLng = document.getElementById('pickupLng').value;
    var dropLat = document.getElementById('dropLat').value;
    var dropLng = document.getElementById('dropLng').value;

    if (!pickupLat || !pickupLng || !dropLat || !dropLng) {
        displayMessage('Please select both pickup and drop locations on the map!', 'danger');
        return false;  // Prevent form submission
    }
    return true;  // Proceed with form submission
}

function checkBro() {
    // After setting the values in hidden fields, add this
    console.log("Pickup Lat: ", document.getElementById('pickupLat').value);
    console.log("Pickup Lng: ", document.getElementById('pickupLng').value);
    console.log("Drop Lat: ", document.getElementById('dropLat').value);
    console.log("Drop Lng: ", document.getElementById('dropLng').value);

}

// Initialize the map over India
var map = L.map('map').setView([20.5937, 78.9629], 5);

// Add OpenStreetMap tile layer
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: 'Map data © OpenStreetMap contributors'
}).addTo(map);

var pickupMarker, dropMarker, route;
var settingPickup = false; // Initial state

// Display messages (alerts)
function displayMessage(message, messageType = 'warning') {
    var messageContainer = document.getElementById('message-container');
    var messageDiv = document.createElement('div');
    messageDiv.className = `alert alert-${messageType} alert-dismissible fade show`;
    messageDiv.role = 'alert';
    messageDiv.innerHTML = `${message} <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>`;
    messageContainer.appendChild(messageDiv);

    setTimeout(function () {
        if (messageDiv) {
            messageDiv.classList.remove('show');
            setTimeout(() => {
                messageDiv.remove();
            }, 500);
        }
    }, 2000);
}

// Remove a marker from the map
function removeMarker(marker) {
    if (marker) {
        map.removeLayer(marker);
    }
}

// Reset map and clear fields
function resetMapToDefault() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var lat = position.coords.latitude;
            var lng = position.coords.longitude;
            map.setView([lat, lng], 13);

            removeMarker(pickupMarker);
            pickupMarker = L.marker([lat, lng]).addTo(map)
                .bindPopup('Pickup location set here!')
                .openPopup();

            getAreaName(lat, lng, function (areaName) {
                document.getElementById('pickup').value = areaName;
                document.getElementById('pickupLat').value = lat;
                document.getElementById('pickupLng').value = lng;
            });

            removeMarker(dropMarker);
            document.getElementById('drop').value = '';
            if (route) map.removeControl(route);
            document.getElementById('distance').value = '';
        });
    } else {
        displayMessage('Geolocation is not supported by this browser.', 'danger');
    }
}

// Validate location in India
function validateLocationInIndia(lat, lng, callback) {
    var url = `https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lng}`;
    fetch(url)
        .then(response => response.json())
        .then(data => {
            var isInIndia = data && data.address && data.address.country === 'India';
            if (!isInIndia) {
                displayMessage('Selected location must be within India!', 'danger');
            }
            callback(isInIndia);
        });
}

// Calculate distance between two coordinates
function calculateDistance(lat1, lng1, lat2, lng2) {
    var R = 6371; // Radius of the earth in km
    var dLat = (lat2 - lat1) * Math.PI / 180;
    var dLng = (lng2 - lng1) * Math.PI / 180;
    var a =
        0.5 - Math.cos(dLat) / 2 +
        Math.cos(lat1 * Math.PI / 180) * Math.cos(lat2 * Math.PI / 180) *
        (1 - Math.cos(dLng)) / 2;
    return R * 2 * Math.asin(Math.sqrt(a)); // Distance in km
}

// Reverse geocoding to get location name
function getAreaName(lat, lng, callback) {
    var url = `https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lng}`;
    fetch(url)
        .then(response => response.json())
        .then(data => {
            if (data && data.display_name) {
                callback(data.display_name);
            } else {
                callback('');
            }
        });
}

// Handle map clicks to select pickup or drop locations
map.on('click', function (e) {
    var lat = e.latlng.lat;
    var lng = e.latlng.lng;

    validateLocationInIndia(lat, lng, function (isInIndia) {
        if (!isInIndia) return;

        if (settingPickup) {
            removeMarker(pickupMarker);
            pickupMarker = L.marker([lat, lng]).addTo(map)
                .bindPopup('Pickup location set here!')
                .openPopup();

            getAreaName(lat, lng, function (areaName) {
                document.getElementById('pickup').value = areaName;
                document.getElementById('pickupLat').value = lat;
                document.getElementById('pickupLng').value = lng;
            });
        } else {
            removeMarker(dropMarker);
            dropMarker = L.marker([lat, lng]).addTo(map)
                .bindPopup('Drop location set here!')
                .openPopup();

            getAreaName(lat, lng, function (areaName) {
                document.getElementById('drop').value = areaName;
                document.getElementById('dropLat').value = lat;
                document.getElementById('dropLng').value = lng;
            });

            var pickupLat = document.getElementById('pickupLat').value;
            var pickupLng = document.getElementById('pickupLng').value;
            if (pickupLat && pickupLng) {
                var distance = calculateDistance(pickupLat, pickupLng, lat, lng);
                if (distance <= 300) {
                    document.getElementById('distance').value = distance.toFixed(2);

                    if (route) map.removeControl(route);
                    route = L.Routing.control({
                        waypoints: [
                            L.latLng(pickupLat, pickupLng),
                            L.latLng(lat, lng)
                        ]
                    }).addTo(map);
                } else {
                    displayMessage('Distance between locations exceeds 300 km!', 'danger');
                }
            }
        }
        settingPickup = !settingPickup;
    });
});

// Swap the pickup and drop locations
function swapLocations() {
    var pickupLocation = document.getElementById('pickup').value;
    var dropLocation = document.getElementById('drop').value;
    document.getElementById('pickup').value = dropLocation;
    document.getElementById('drop').value = pickupLocation;

    var pickupLat = document.getElementById('pickupLat').value;
    var pickupLng = document.getElementById('pickupLng').value;
    var dropLat = document.getElementById('dropLat').value;
    var dropLng = document.getElementById('dropLng').value;
    document.getElementById('pickupLat').value = dropLat;
    document.getElementById('pickupLng').value = dropLng;
    document.getElementById('dropLat').value = pickupLat;
    document.getElementById('dropLng').value = pickupLng;

    // Reset the route after swapping
    if (route) map.removeControl(route);
    if (pickupLat && pickupLng && dropLat && dropLng) {
        route = L.Routing.control({
            waypoints: [
                L.latLng(pickupLat, pickupLng),
                L.latLng(dropLat, dropLng)
            ]
        }).addTo(map);
    }
}

// Initialize map on page load
resetMapToDefault();