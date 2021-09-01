

function Silo(canvas, data)
{
    this.ID = -1;
    this.canvas = canvas;
    this.ctx = this.canvas.getContext("2d");
    this.cw = this.canvas.width;
    this.ch = this.canvas.height;
    this.ctx.lineCap = "round";
    this.y = this.ch - 10;
    this.drawingColor = "#333538";
    this.a = 0;
    this.data = data;
    this.b = data.MaxCapacity;
    this.weight = new Weight();
    this.prev_weight = null;
    this.IsInLoadState = false;
    this.IsInUnloadState = false;
    this.LoadStateChanged = null;
    this.UnloadStateChanged = null;
    this.LogWeight = null;
    this.LogWeightInternval = null;
    this.LoadStateInterval = null;
    this.UnloadStateInterval = null;
}

Silo.prototype.start_weight = function(elem, obj)
{
    this.stop_weight();
    this.weight.ScaleID = this.data.ScaleID;
    this.weight.WeightDisplay = elem;
    this.weight.WeightChanged = function (val) {
            obj.prev_weight = obj.a;
        obj.a = val;
    };

    if (obj.data.IsLoad == true) {
        LoadStateInterval = setInterval(function () {
            if (obj.a - obj.prev_weight >= obj.data.LoadInterval) {
                if (obj.IsInLoadState == false) {
                    obj.IsInLoadState = true;
                    if (obj.LoadStateChanged != null)
                        obj.LoadStateChanged(obj.IsInLoadState)
                }
            }
            else if (obj.IsInLoadState == true) {
                obj.IsInLoadState = false;
                if (obj.LoadStateChanged != null)
                    obj.LoadStateChanged(obj.IsInLoadState);
            }
        }, obj.data.LoadIntervalTime);
    }

    if (obj.data.IsUnload == true) {
        UnloadStateInterval = setInterval(function () {
            if (obj.prev_weight - obj.a >= obj.data.UnloadInterval) {
                if (obj.IsInUnloadState == false) {
                    obj.IsInUnloadState = true;
                    if (obj.UnloadStateChanged != null)
                        obj.UnloadStateChanged(obj.IsInUnloadState)
                }
            }
            else if (obj.IsInUnloadState == true) {
                obj.IsInUnloadState = false;
                if (obj.UnloadStateChanged != null)
                    obj.UnloadStateChanged(obj.IsInUnloadState)
            }
        }, obj.data.UnloadIntervalTime);
    }

    if (obj.data.IsLogWeight == true) {
        obj.LogWeightInternval = setInterval(function () {
            if (obj.LogWeight != null)
                obj.LogWeight(null)
        }, obj.data.LogWeightTime);
    }
        
    

    this.weight.Start(this.weight);
}

Silo.prototype.stop_weight = function () {
    this.weight.Stop();
    clearInterval(this.UnloadStateInterval);
    clearInterval(this.LoadStateInterval);
    clearInterval(this.LogWeightInternval);
}

Silo.prototype.animate = function ()
{
    //if (y > (1-(a/b))*ch) {
    requestAnimationFrame(this.animate.bind(this));
    //}
    
    var grd = this.ctx.createLinearGradient(0, 0, this.cw, this.ch);
    grd.addColorStop(0, "#333538");
    grd.addColorStop(0.3, "#777a7f");
    grd.addColorStop(0.4, "#777a7f");
    grd.addColorStop(0.5, "#e3e9f2");
    grd.addColorStop(0.6, "#777a7f");
    grd.addColorStop(0.7, "#777a7f");
    grd.addColorStop(1, "#333538");

    this.ctx.clearRect(0, 0, this.cw, this.ch);
    this.ctx.save();
    this.drawContainer(0, null, null);
    this.drawContainer(10, "#333538", null);
    

    
    this.drawContainer(5, grd, grd);
    this.ctx.save();
    this.ctx.clip();
    this.drawLiquid();
    this.ctx.restore();
    this.ctx.restore();
    var value = Math.floor((1 - (this.a / this.b)) * (this.ch))
    if (this.y > value)
        this.y--;
    else if (this.y < value)
        this.y++;
}

Silo.prototype.drawLiquid = function () {
    this.ctx.beginPath();
    this.ctx.moveTo(0, this.y);
    

    for (var x = 0; x < this.cw; x += 25) {
        this.ctx.quadraticCurveTo(x + 25, this.y , x + 25, this.y);
    }
    this.ctx.lineTo(this.cw, this.ch);
    this.ctx.lineTo(0, this.ch);
    this.ctx.closePath();
    this.ctx.fillStyle = this.drawingColor;
    this.ctx.fill();
}

Silo.prototype.drawContainer = function(linewidth, strokestyle, fillstyle) {
    this.ctx.beginPath();
    this.ctx.moveTo(10, 20);
    //ctx.bezierCurveTo(121, 36, 133, 57, 144, 78);
        this.ctx.bezierCurveTo(10, 0, this.cw, 0, this.cw - 10 , 20);
    this.ctx.rect(10, 25, this.cw - 18, this.ch - 60);
    this.ctx.moveTo(10, this.ch - 30);
    this.ctx.lineTo(50, this.ch - 5);
    this.ctx.lineTo(this.cw - 50, this.ch-6);
    this.ctx.lineTo(this.cw - 8, this.ch - 30);

    this.ctx.moveTo(10, this.ch - 30);
    this.ctx.lineTo(10, this.ch - 6);
    this.ctx.moveTo(this.cw - 8, this.ch - 30);
    this.ctx.lineTo(this.cw - 8, this.ch - 6);
    //ctx.bezierCurveTo(160, 109, 176, 141, 188, 175);
    //ctx.bezierCurveTo(206, 226, 174, 272, 133, 284);
    //ctx.bezierCurveTo(79, 300, 24, 259, 25, 202);
    //ctx.bezierCurveTo(25, 188, 30, 174, 35, 161);
    //ctx.bezierCurveTo(53, 115, 76, 73, 100, 31);
    //ctx.bezierCurveTo(103, 26, 106, 21, 109, 15);
    this.ctx.lineWidth = linewidth;
    this.ctx.strokeStyle = strokestyle;
    this.ctx.stroke();
    if (fillstyle) {
        this.ctx.fillStyle = fillstyle;
        this.ctx.fill();
    }
}