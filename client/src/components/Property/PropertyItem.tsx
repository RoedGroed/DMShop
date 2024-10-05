import { PropertyDto } from "../../Api.ts";
import React from "react";


interface PropertyItemProps {
    property: PropertyDto;
}

const PropertyItem: React.FC<PropertyItemProps> = ({ property }) => {
    return (
        <li className="p-1 bg-gray-900 rounded-lg flex justify-between items-center text-white">
            <p className="flex-1">{property.propertyName}</p>
        </li>
    );
};

export default PropertyItem;
