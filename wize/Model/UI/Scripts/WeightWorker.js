var weightWorker;
var element

function startWorker(guid, elem) {
   
    element = elem;
    if (typeof (Worker) !== "undefined") {
        if (typeof (weightWorker) == "undefined") {
            weightWorker = new Worker("../Scripts/ViewScripts/Scales/Weight.js");
        }
        weightWorker.onmessage = function (event) {
            
            console.log(event.data);
            var obj = JSON.parse(event.data);

            element.text(obj.msg);
            console.log(event.data);
        };
        
        weightWorker.postMessage(new WorkerMessage('start_weight',guid));
    } else {
        console.log("Sorry, your browser does not support Web Workers...");
    }
  
}

function stopWorker() {
    if (typeof (weightWorker) !== "undefined") {
        weightWorker.terminate();
        weightWorker = undefined;
    }

}

function WorkerMessage(cmd,guid)
{
    this.cmd = cmd;
    this.guid = guid;
}