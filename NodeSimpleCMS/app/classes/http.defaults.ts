export class HttpDefaults {

    public url: string; 

    public data: string;

    public contentType: string; 

    public callback: Object; 

    public includeKey: boolean;

    constructor() {
        this.url = "";
        this.data = null;
        this.contentType = "application/json";
        this.includeKey = false;
    }

}