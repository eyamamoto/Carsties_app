'use server'

import { Auction, PagedResult } from "../types";

//carregar dados
export async function getData(pageNumber:number, pageSize:number): Promise<PagedResult<Auction>>{
    const res = await fetch( `http://localhost:6001/search?pagesize=${pageSize}&pageNumber=${pageNumber}` );

    if(!res.ok) throw new Error('Fail to fetch error');

    return res.json();
}
