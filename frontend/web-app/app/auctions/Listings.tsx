/* eslint-disable @typescript-eslint/no-unused-vars */
'use client'


import React, { Fragment, useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import AppPagination from '../components/AppPagination';
import { Auction, PagedResult } from '../types';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';
import { useParamsStore } from '../hooks/useParamsStore';
import { useShallow } from 'zustand/shallow';
import qs from 'query-string'
import EmptyFilter from '../components/EmptyFilter';
import { useAuctionStore } from '../hooks/useActionStore';

export default function Listings() {
    // const [auctions, setAuctions] = useState<Auction[]>([]);
    // const [pageCount, setPageCount] = useState(0);
    // const [pageNumber, setPageNumber] = useState(1);
    // const [pageSize, setPagesize] = useState(4);
    // const [data, setData] = useState<PagedResult<Auction>>();

    const [loading, setLoading]= useState(true);
    
    const params = useParamsStore(
        useShallow( 
            state => ({
                pageNumber:state.pageNumber,
                pageSize:state.pageSize,
                searchTerm:state.searchTerm,
                orderBy:state.orderBy,
                filterBy: state.filterBy,
                seller:state.seller,
                winner:state.winner
            })
        )
    );

    const data = useAuctionStore(
        useShallow( 
            state => ({
            auctions:state.auctions,
            totalCount: state.totalCount,
            pageCount:state.pageCount
        }))
    )

    const setData = useAuctionStore(state => state.setData);

    const setParams = useParamsStore(state => state.setParams);
    const url = qs.stringifyUrl({url:'', query:params})

    function setPageNumber(pageNumber:number){
        setParams({pageNumber})
    }
    
    useEffect(() =>{
        getData(url).then(data =>{
            setData(data)
            setLoading(false);
        })
    },[setData, url])

    if(!data) return <h3>Loading ... </h3>

    return (
        <Fragment>
            <Filters />
            {data.totalCount === 0 ? (
                <EmptyFilter showReset />
            ): (
                <>
                     <div className='grid grid-cols-4 gap-5'>
                        {data.auctions.map((auction) => (
                            <AuctionCard key={auction.id} auction={auction} />
                        ))}
                    </div>
                    <div className='flex justify-center mt-4'>
                        <AppPagination pageChanged={setPageNumber} currentPage={params.pageNumber} pageCount={data.pageCount}/>
                    </div>
                </>
            )}
        </Fragment>
    )
}
