
function Weight()
{
    this.ScaleID = 0;
    this.WeightDisplay = null;
    this.WeightChanged = null;
    this.Websocket = null;
    this.Start = function (obj) {
        //var uri = "ws://localhost:50587/api/WeightSocket/";
        //var uri = "ws://fansrating.com/api/WeightSocket/";
        var uri = "wss://mishkalim.co.il/api/WeightSocket/";
        //TODO
        //Initialize socket  
        this.Websocket = new WebSocket(uri);
        //Open socket and send message  
        this.Websocket.onopen = function () {
            console.log('<div>Connected to server.</div>');
            
            if (obj.ScaleID != null && obj.ScaleID > 0)
                this.send(obj.ScaleID);

        };

        //Socket error handler  
        this.Websocket.onerror = function (event) {
            console.log('<div>Ooooops! Some error occured</div>');
        };

        this.Websocket.onmessage = function (event) {
            if (obj.WeightDisplay != null)
                obj.WeightDisplay.text(event.data);
            if (obj.WeightChanged != null)
                obj.WeightChanged(event.data);
        };

        this.Websocket.onclose = function () { console.log("Closing") };
    }

    this.Stop = function () {
        if (this.Websocket != null) {
            console.log("Closing Event");
            this.Websocket.close();
        }
        this.Websocket = null;
    }
}



/*

var getWeightIntervel
self.onmessage = function(msg)
{
    console.log(msg);
    switch(msg.data.cmd)
    {
        case 'start_weight':
            getWeightIntervel = setInterval(function () {
                GetWeight(msg.data.guid);
            }, 1000);
            break;
        case 'stop_weight':
            clearInterval(getWeightIntervel);
            break;
    }

}


function GetWeight(guid) {
    console.log(guid);
    if (guid == null)
        guid = 0;
    console.log(guid);
    console.log(guid.length);

    
    var xhr = new XMLHttpRequest();
    xhr.open("POST", '/Scales/Weight?guid='+guid, true);
    //xhr.overrideMimeType("application/json; charset=utf-8");
    xhr.setRequestHeader("Content-type", "application/json; charset=utf-8");
    

    xhr.onreadystatechange = function () {
        console.log(xhr.responseText);
            postMessage(xhr.responseText);
    }
    xhr.send();

    
};*/