'use client'

import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { ReactNode, useEffect, useRef } from 'react'
import { useAuctionStore } from '../hooks/useActionStore'
import { useBidStore } from '../hooks/useBidStore'
import { useParams } from 'next/navigation'
import { Bid } from '../types'

type Props = {
    children: ReactNode
}

export default function SignalRProvider({children}:Props) {

    const connection = useRef<HubConnection | null>(null);
    const setCurrentPrice = useAuctionStore(state => state.setCurrentPrice);
    const addBid = useBidStore(state => state.addBid)
    const params = useParams<{id:string}>();

    useEffect(() => {
        if(!connection.current){

            connection.current = new HubConnectionBuilder()
                .withUrl('http://localhost:6001/notifications')
                .withAutomaticReconnect()
                .build();
            
            connection.current.start()
                .then(() => 'connection to notification hub')
                .catch((err: any) => console.log(err));

            connection.current.on('BidPlaced', (bid: Bid) =>{
                setCurrentPrice(bid.auctionId, bid.amount)
            })
        }
    },[setCurrentPrice])

    return (
        children
    )
}
