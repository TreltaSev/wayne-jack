from typing import Dict

from pydantic import BaseModel


class Chip(BaseModel):
    denomination: int
    min: int
    
    @property
    def id(self) -> str:
        return f"chip-{self.denomination}"
    
    def __str__(self) -> str:
        return self.id

class Chips:    
    denominations: list[Chip] = [
        Chip(denomination=1, min=10),
        Chip(denomination=5, min=5),
        Chip(denomination=10, min=5),
        Chip(denomination=20, min=3),
        Chip(denomination=50, min=2)
    ]
    
    @classmethod
    def calculate_from(cls, balance: int):
        
        # Create ascending and descending copies of denominations
        ascending = list(cls.denominations)
        descending = reversed(cls.denominations)
        
        output = {}
        
        remaining = int(balance)
        
        
        # First Pass, Minimum Catch
        for chip in ascending:
            possible = remaining // chip.denomination            
            amount = min(possible, chip.min)
            remaining -= amount * chip.denomination
            output[chip.id] = amount
        
        # Second Pass, Total Catch
        for chip in descending:
            possible = remaining // chip.denomination
            remaining -= possible * chip.denomination
            output[chip.id] += possible
            
        return output
            
        
        


class Player(BaseModel):
    balance: int
    
    
    
player = Player(balance=1500)

print(Chips.calculate_from(player.balance))
    