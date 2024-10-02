'use server'

import { Auction, PagedResult } from "../types";

//carregar dados
export async function getData(query:string): Promise<PagedResult<Auction>>{
    const res = await fetch( `http://localhost:6001/search${query}` );

    if(!res.ok) throw new Error('Fail to fetch error');

    return res.json();
}
