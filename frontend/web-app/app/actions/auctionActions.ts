'use server'

import { auth } from "@/auth";
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

    //recupera sessão do usuario
    const session = await auth();

    const res = await fetch('http://localhost:6001/auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c', {
        method:'PUT',
        headers:{
            "Content-type" : "application/json",
            "Authorization" : "Bearer " + session?.accessToken
        },
        body: JSON.stringify(data)
    });
    if(!res.ok) return{status: res.status, message: res.statusText}
    return res.statusText;
}