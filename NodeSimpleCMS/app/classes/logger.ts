import { ErrorMessages }  from "./error.messages";
import { Injectable }     from "@angular/core";


@Injectable()
export class Logger {

    public static LogMessage(message: string): void {
        if (message.length > 0) {
            console.log("API Message: " + message);
        }
    
    }

    public static LogMessages(messages: string[]): void {
        for (var i = 0, message; message = messages[i++];) {
            Logger.LogMessage(message);
        }
    }

    public static LogError(error: string): any {
        if (error.length > 0) {
            console.log("%CAPI Error: " + error, "color: red");
        }
    }

    public static LogErrors(errors: string[]): void {
        for (var i = 0, error; error = errors[i++];) {
            Logger.LogError(error);
        }
    }
}