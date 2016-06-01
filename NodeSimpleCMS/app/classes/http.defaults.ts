export class HttpDefaults {

    public url: string; 

    public data: string;

    public contentType: string; 

    public callback: Object; 

    constructor() {
        this.url = "";
        this.data = null;
        this.contentType = "application/json";
    }

}