'use server'


import { Auction, PagedResult } from "../types";
import { fetchWrapper } from "../libFetch/fetchWrapper";
import { FieldValues } from "react-hook-form";
import { revalidatePath } from "next/cache";

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

export async function updateAction(data:FieldValues, id:string) {
    const res = await fetchWrapper.put(`auctions/${id}`, data)
    //revalidate evita o cache para evitar problemas com diferença de dados na tela ao atualizar o formulário
    revalidatePath(`/auctions/${id}`)
    return res;
}

export async function deleteAuction(id:string){
    return await fetchWrapper.del(`auctions/${id}`)
}

//auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c