import {useAtom} from "jotai";
import {OrdersAtom} from "../atoms/OrdersAtom";
import {useEffect} from "react";
import {http} from "../http.ts";

export function useInitializeData() {
    const [, setOrders] = useAtom(OrdersAtom);


    useEffect(() => {
        http.api.orderGetAllOrders({limit: 10, startAt: 0}).then((response) => {
            setOrders(response.data);
        }).catch(e => {
            console.log("Failed to load orders", e);
        })
    }, [])
}   