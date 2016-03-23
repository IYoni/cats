﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IShippingInstructionService
    {
         bool AddShippingInstruction(ShippingInstruction shippingInstruction);
         bool EditShippingInstruction(ShippingInstruction shippingInstruction);
         bool DeleteShippingInstruction(ShippingInstruction shippingInstruction);
         bool DeleteById(int id);
         List<ShippingInstruction> GetAllShippingInstruction();
         ShippingInstruction FindById(int id);
         List<ShippingInstruction> FindBy(Expression<Func<ShippingInstruction, bool>> predicate);
        ShippingInstruction GetSiNumber(string siNumber);
        int GetShipingInstructionId(string si);

    }
}
