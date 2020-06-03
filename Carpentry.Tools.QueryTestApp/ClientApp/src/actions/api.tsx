export const api = {
    async callTestQuery(param: InventoryQueryParameter): Promise<InventoryOverviewDto[]> {
        const endpoint = `api/App/CallTestQuery`;
        const result = await Post(endpoint, param);
        return result;
    },
}

async function Get(url: string): Promise<any> {
    const response = await fetch(url);
    if (response.status === 202) {
        return;
    }
    const result = await response.json();
    return result;
}

async function Post(endpoint: string, payload: any): Promise<any> {
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

