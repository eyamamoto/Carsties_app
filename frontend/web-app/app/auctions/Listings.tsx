import React from 'react'
import AuctionCard from './AuctionCard';

//carregar dados
async function getData(){
    const res = await fetch('http://localhost:6001/search?pagesize=10');

    if(!res.ok) throw new Error('Fail to fetch error');

    return res.json();
}

export default async function Listings() {

    const data = await getData();

    return (
        <div className='grid grid-cols-4 gap-5'>
            {data && data.results.map((auction:any) => (
                <AuctionCard key={auction.id} auction={auction} />
            ))}
        </div>
    )
}
