export class ErrorMessages {

    public static passwordLength(): string {
        return "Password must be at least 5 characters long";
    }

    public static incorrectOldPassword(): string {
        return "Old password is not correct";
    }

    static passwordNotMatch(): string {
        return "Confirm password does not match new password";
    }

    public static invalidNumber(): string {
        return "&nbsp;&nbsp;You did not enter a number";
    }

    public static nameLength(): string {
        return "The name field must be at least 3 characters long"
    }

    public static email(): string {
        return "The email must be of the format address@example.com";
    }

    public static title(): string {
        return "The title field is required";
    }

    public static content(): string {
        return "The content field is required";
    }

}