export class Post {
    id: number; 
    
    title: string;
    
    content: string; 

    created: Date; 

    visible: boolean;
    
    attachment: boolean; 
    
    attachmentPath: string; 
    
    comments: string[]; 

    constructor(id?: number, title?: string, content?: string, created?: Date, visible?: boolean, attachment?: boolean) {
        if (id != null && visible != null) {
            this.id = id;
            this.title = title;
            this.content = content;
            this.created = created;
            this.visible = visible;
            this.attachment = attachment; 
        }
    }
}