'use server'

import { Auction, PagedResult } from "../types";

//carregar dados
export async function getData(query:string): Promise<PagedResult<Auction>>{
    const res = await fetch( `http://localhost:6001/search${query}` );

    if(!res.ok) throw new Error('Fail to fetch error');

    return res.json();
}

//teste em endpoint com autenticação
export async function updateAuctionTest(){
    const data = {
        mileage: Math.floor(Math.random() * 10000) + 1
    }

    const res = await fetch('http://localhost:6001/auctions', {
        method:'PUT',
        headers:{

        },
        body: JSON.stringify(data)
    });
    if(!res.ok) return{status: res.status, message: res.statusText}
    return res.json();
}