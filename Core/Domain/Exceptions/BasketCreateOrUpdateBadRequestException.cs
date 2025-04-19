﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BasketCreateOrUpdateBadRequestException() 
        : BadRequestException($"Invalid Operation When Crate Or Update Basket !")
    {
    }
}
