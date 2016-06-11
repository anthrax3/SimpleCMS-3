import { ErrorMessages }  from "./error.messages";
import { Injectable }     from "@angular/core";


@Injectable()
export class Logger {

    public static LogMessages(messages: string[]): void {
        for (var i = 0, message; message = messages[i++];) {
            console.log("API Message: " + message); 
        }
    }

    public static LogErrors(errors: string[]): void {
        for (var i = 0, error; error = errors[i++];) {
            console.log("%CAPI Error: " + error, "color:red"); 
        }
    }
}