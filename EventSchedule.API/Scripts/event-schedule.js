var event;

window.addEventListener('load', (e) => {

    $.ajax({
        url: '/api/events/0ef45384-bf65-46da-bd0f-d6e80287ce2d/Details',
        type: 'GET',
        dataType: 'json',
        data: [],
        success: function (data, textStatus, xhr) {
            event = data;
            loadEvent();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    });

});

function loadMap() {

}

function loadEvent() {
    console.log(event);

    var stages = event.Stages;
    console.log(stages);

    var canvas = document.getElementById("cvs");
    var rect = canvas.getBoundingClientRect();
    var dpr = 1;
    canvas.width = rect.width * dpr;
    canvas.height = rect.height * dpr;
    var ctx = canvas.getContext("2d");
    ctx.scale(dpr, dpr);
    canvas.onclick = function (e) {
        if (e.region) {
            alert('You clicked ' + e.region);
        }
    }

    var startDate = new Date(event.StartDate);
    var endDate = new Date(event.EndDate);

    var fontSize = 14;

    var hourX = 50;
    var hourY = 10;
    var hourHeight = 50;
    var hourWidth = 300;

    var stageX = 10;
    var stageY = 60;
    var stageHeight = 75;
    var stageWidth = 120;


    ctx.font = fontSize + 'px Arial';

    var hourCounter = 0;
    var availableEvent = true;
    var eventHours = new Date(startDate);

    while (availableEvent) {
        ctx.strokeStyle = "#cb9d41";
        ctx.beginPath();
        ctx.moveTo(hourX + (hourCounter * hourWidth), hourY);
        ctx.lineTo(hourX + (hourCounter * hourWidth), 1000); // find height
        ctx.stroke();

        ctx.font = 'bolder ' + 16 + 'px Arial';
        ctx.fillStyle = '7d6128';
        ctx.fillText(getPrettyHour(eventHours), hourX + (hourCounter * hourWidth) + 5, hourY + (hourHeight / 2) + (fontSize / 2));
        ctx.stroke();

        hourCounter++;
        eventHours = new Date(addHours(eventHours, 1));
        if (eventHours > endDate) {
            availableEvent = false;
        }
    }

    var eventRectangles = [];
    ctx.font = '' + 12 + 'px Arial';
    ctx.font = fontSize + 'px Arial';
    for (var i = 0; i < stages.length; i++) {
        for (let j = 0; j < 10; j++) {
            ctx.fillStyle = '#d20047';
            ctx.fillText(stages[i].Name, stageX + (250 * j), stageY + (i * stageHeight) + fontSize);
            ctx.stroke();

            stages[i].Presentations.forEach(presentation => {
                var eventLength = getDateDiff(new Date(presentation.EndDate), new Date(presentation.StartDate));
                ctx.beginPath();
                ctx.fillStyle = '#d20047';

                var eventRectangle = {
                    x: hourX + (getDateDiff(new Date(presentation.StartDate), startDate) * hourWidth),
                    y: stageY + (stageHeight * (stages[i].Order - 1)) + (fontSize * 1.5),
                    width: (hourWidth * eventLength),
                    height: stageHeight - (fontSize * 2.5),
                    stage: stages[i].Name,
                    artist: presentation.Artist.Name,
                    time: getPrettyHour(new Date(presentation.StartDate)) + ' - ' + getPrettyHour(new Date(presentation.EndDate))
                };

                ctx.rect(
                    eventRectangle.x,
                    eventRectangle.y,
                    eventRectangle.width,
                    eventRectangle.height);

                ctx.fill();
                ctx.strokeStyle = "#85002d";
                ctx.stroke();

                eventRectangles.push(eventRectangle);

                ctx.font = 'bolder ' + 14 + 'px Arial';
                ctx.fillStyle = 'white';
                ctx.fillText(
                    presentation.Artist.Name,
                    hourX + (getDateDiff(new Date(presentation.StartDate), startDate) * hourWidth) + (fontSize / 2),
                    stageY + (stageHeight * (stages[i].Order - 1)) + (fontSize * 3));

                ctx.font = 'bold ' + 11 + 'px Arial';
                ctx.fillText(
                    getPrettyHour(new Date(presentation.StartDate)) + '-' + getPrettyHour(new Date(presentation.EndDate)),
                    hourX + (getDateDiff(new Date(presentation.StartDate), startDate) * hourWidth) + (fontSize / 2),
                    stageY + (stageHeight * (stages[i].Order - 1)) + (fontSize * 4));

                ctx.font = fontSize + 'px Arial';
            });

        }
    }

    ctx.font = fontSize + 'px Arial';

    function getDateDiff(date1, date2) {
        return Math.abs(date1 - date2) / 36e5;
    }

    function getPrettyHour(date) {
        return date.toLocaleTimeString(navigator.language, {
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    function addHours(date, hours) {
        return date.setTime(date.getTime() + (hours * 60 * 60 * 1000));
    }

    function getMousePosition(canvas, e) {
        let rect = canvas.getBoundingClientRect();
        let x = e.clientX - rect.left;
        let y = e.clientY - rect.top;

        eventRectangles.forEach(rectangle => {

            if (rectangle.x < x && rectangle.y < y && rectangle.x + rectangle.width > x && rectangle.y + rectangle.height > y) {
                $('#eventStage').html(rectangle.stage);
                $('#eventArtist').html(rectangle.artist);
                $('#eventTime').html(rectangle.time);
                $('#exampleModal').modal('show');
            }
        });
    }

    canvas.addEventListener("mousedown", function (e) {
        getMousePosition(canvas, e);
    });
}