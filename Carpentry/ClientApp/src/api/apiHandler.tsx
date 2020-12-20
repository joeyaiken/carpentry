//Some functions to help with making API Calls

export async function Get(url: string): Promise<any> {
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

export async function GetFile(url: string): Promise<any> {
    // console.log(`get fetching URL ${url}`)
    const response = await fetch(url);
    // console.log('updating tracked sets ping 7?');
    // console.log(response);

    // const contentType = response.headers.get("content-type");
    // if(contentType && contentType.indexOf("application/json") !== -1){
    //     const result = await response.json();
    //     return result;
    // }

    if(response.status !== 200){
        //error
        return;
    } else {
        return response.blob();
    }


    /*
    fetch('api/zip')
        .then((response) => {
            if (response.status != 200) {
                let errorMessage = "Error processing the request... (" + response.status + " " + response.statusText + ")";
                throw new Error(errorMessage);
            } else {
                return response.blob();
            }
        })
        .then((blob: any) => {
            // !!! see next code block !!!
            downloadData('geojsons.zip', blob);
        })
        .catch(error => {
            console.error(error);
        });
    
    */



    // if (response.status === 202) {
    //     return;
    // }
    // const result = await response.json();
    // return result;
}

export async function Post(endpoint: string, payload: any): Promise<any> {
    // console.log('post');
    // console.log(payload);
    const bodyToAdd = JSON.stringify(payload);
    const response = await fetch(endpoint, {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: bodyToAdd
    });
    const result = await response.json().catch(() => {
        return;
    });
    return result;
}

