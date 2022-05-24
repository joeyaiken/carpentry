//Some functions to help with making API Calls
export async function Get(url: string): Promise<any> {
  const response = await fetch(url);
  const contentType = response.headers.get("content-type");
  if (contentType && contentType.indexOf("application/json") !== -1) {
    return await response.json();
  } else if(contentType && contentType.indexOf("text/plain") !== -1) {
    return await response.text();
  }
  return;
}

export async function GetFile(url: string): Promise<any> {
  const response = await fetch(url);
  if (response.status !== 200) {
    return;
  } else {
    return response.blob();
  }
}

export async function Post(endpoint: string, payload: any): Promise<any> {
  const bodyToAdd = JSON.stringify(payload);
  const response = await fetch(endpoint, {
    method: 'post',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: bodyToAdd
  });
  return await response.json().catch(() => {
    return;
  });
}

