var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var HelloMessage = (function (_super) {
    __extends(HelloMessage, _super);
    function HelloMessage(props, context) {
        _super.call(this, props, context);
        this.state = {
            name: '',
        };
    }
    HelloMessage.prototype.render = function () {
        return React.createElement("h1", null, 
            "hello ", 
            this.state.name);
    };
    return HelloMessage;
}(React.Component));
ReactDOM.render(React.createElement(HelloMessage, { name: "React" }), document.getElementById('container'));
