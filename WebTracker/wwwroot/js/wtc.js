var trackerBaseUrl = 'https://localhost:7239/home/track';
var app = 'TestApp';

$(function () {
    var link = $(location).attr('href');
    var url = new URL(link);
    var params = url.searchParams.toString();

    // Create a new XMLHttpRequest
    var xhr = new XMLHttpRequest();
    xhr.open('POST', trackerBaseUrl + '?' + params);  // Replace with your URL

    // Set the request header
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');

    // Send the page URL as the POST data
    xhr.send('app=' + app + '&' + 'pageView=' + encodeURIComponent(link));
});

document.addEventListener('click', function (event) {
    var target = event.target;
    if (target.tagName.toLowerCase() === 'a') {  // If the clicked element is a link
        event.preventDefault();  // Prevent the default action (navigating to the link)

        var link = target.href;  // Get the link URL
        var url = new URL(link);
        var params = url.searchParams.toString();

        // Create a new XMLHttpRequest
        var xhr = new XMLHttpRequest();
        xhr.open('POST', trackerBaseUrl + '?' + params);  // Replace with your URL

        // Set the request header
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');

        // Send the link as the POST data
        xhr.send('app=' + app + '&' + 'linkClick=' + encodeURIComponent(link));
    }
});


