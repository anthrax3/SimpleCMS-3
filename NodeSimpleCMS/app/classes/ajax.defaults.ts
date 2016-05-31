export class AjaxDefaults {

    public url: string; 

    public data: string; 

    public type: string;

    public contentType: string; 

    public success: Object; 

    public error: Object; 

    public async: boolean; 

    constructor() {
        this.url = "";
        this.data = null;
        this.contentType = "application/json";
        this.async = true; 
    }

}