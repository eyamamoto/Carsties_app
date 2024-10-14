
import { numberWithCommas } from '@/app/lib/numberWithComma';
import { Bid } from '@/app/types'
import { format } from 'date-fns';
import React from 'react'

type Props = {
    bid:Bid
}

export default function BidItem({bid}:Props) {

    //muda de cor de acordo com o bid recebido
    function getBidInfo(){
        let bgcolor = '';
        let text = '';
        switch (bid.bidStatus) {
            case 'Accepted':
                bgcolor = 'bg-green-200'
                text = 'Bid accepted'
                break;
            case 'AcceptedBelowReserve':
                bgcolor = 'bg-amber-500'
                text = 'Reserve not met'
                break;
            case 'TooLow':
                bgcolor = 'bg-red-200'
                text = 'Bid was too low'
                break;
            default:
                bgcolor='bg-red-200'
                text='bid placed after auction finished'
                break;
        }
        return {bgcolor, text}
    }
    return (
        <div className={`
            border-gray-300 border-2 px-3 py-2 rounded-lg flex justify-between items-center mb-2
            ${getBidInfo().bgcolor}
        `}>
            <div className='flex flex-col'>
                <span>Bidder: {bid.bidder}</span>
                <span className='text-gray-700 text-sm'>
                    Time:  { format( new Date(bid.bidTime), 'dd MMM yyyy h:mm a')}
                </span>
            </div>
            <div className='flex flex-col text-right'>
                <div className='text-xl font-semibold'>
                    ${numberWithCommas(bid.amount) }
                </div>
                <div className='flex flex-row items-center'>
                    <span>
                        {getBidInfo().text}
                    </span>
                </div>
            </div>
        </div>
    )
}
