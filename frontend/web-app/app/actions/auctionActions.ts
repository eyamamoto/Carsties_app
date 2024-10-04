'use server'


import { Auction, PagedResult } from "../types";
import { fetchWrapper } from "../libFetch/fetchWrapper";
import { FieldValues } from "react-hook-form";

//carregar dados
export async function getData(query:string): Promise<PagedResult<Auction>>{
    return await fetchWrapper.get(`search${query}`)
}

//teste em endpoint com autenticação
export async function updateAuctionTest(){
    const data = {
        mileage: Math.floor(Math.random() * 10000) + 1
    }
    return await fetchWrapper.put('auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c', data);
}

export async function createAuction(data:FieldValues){
    return await fetchWrapper.post('auctions',data);
}

export async function getDetailedViewData(id:string): Promise<Auction>{
    return await fetchWrapper.get(`auctions/${id}`);
}

//auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c