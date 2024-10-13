import { create } from "zustand";
import { Auction, PagedResult } from "../types"

//estado 
type State = {
    auctions: Auction[]
    totalCount: number;
    pageCount: number
}

//estado para armazenar dados e alterar preço do leilão
type Auctions = {
    setData: (data: PagedResult<Auction>) => void
    setCurrentPrice: (auctionId:string, amount: number) => void
}

const initialState: State = {
    auctions:[],
    pageCount:0,
    totalCount:0
}

//configura o estado inicial
export const useAuctionStore = create<State & Auctions>((set) => ({
    ...initialState,

    setData: (data: PagedResult<Auction>) =>{
        set(() => ({
            auctions:data.results,
            totalCount:data.totalCount,
            pageCount:data.pageCount
        }))
    },

    //altera os auctions de dentro do state
    setCurrentPrice: (auctionId:string, amount:number) => {
        set((state) => ({
            auctions: state.auctions.map((auction) => auction.id === auctionId 
                ? {...auction, currentHighBid:amount} 
                : auction
            ) 
        }))
    }

}))

