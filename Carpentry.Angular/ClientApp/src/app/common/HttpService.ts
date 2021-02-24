import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";


export abstract class HttpService {
    constructor(
        // protected http: HttpClient, public baseUrl: string
        ) {
        
    }

    async Get(url: string): Promise<any> {
        // console.log(`get fetching URL ${url}`)
        const response = await fetch(url);
        // console.log('updating ...something');
        // console.log(response);
    
        const contentType = response.headers.get("content-type");
        if(contentType && contentType.indexOf("application/json") !== -1){
            const result = await response.json();
            return result;
        } else if(contentType && contentType.indexOf("text/plain") !== -1){
            const result = await response.text();
            return result;
        }
        
        return;
    
        // if (response.status === 202) {
        //     return;
        // }
        // const result = await response.json();
        // return result;
    }

    // async Get(url: string): Observable<any> {
    //     fetch(url).then(response => {

    //     })
    // }
    
    // async Post(endpoint: string, payload: any): Promise<any> {
    //     // console.log('post');
    //     // console.log(payload);
    //     const bodyToAdd = JSON.stringify(payload);
    //     const response = await fetch(endpoint, {
    //         method: 'post',
    //         headers: {
    //             'Accept': 'application/json',
    //             'Content-Type': 'application/json'
    //         },
    //         body: bodyToAdd
    //     });
    //     const result = await response.json().catch(() => {
    //         return;
    //     });
    //     return result;
    // }
    


}