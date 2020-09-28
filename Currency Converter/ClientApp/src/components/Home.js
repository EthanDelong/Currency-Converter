import React, { Component } from 'react';

export class Home extends Component
{
    static displayName = Home.name;
    constructor(props)
    {
        super(props);
        this.state = { currencies: [], fromCurrency: '', toCurrency: '', rate: 0, fromValue: 1, toValue: 1 };
    }

    componentDidMount()
    {
        this.getAvailableCurrencies();
    }

    changeFrom = fromCurrency =>
    {
        this.setState({ fromCurrency }, () => Home.setExchangeRate())
    };

    changeTo = toCurrency =>
    {
        this.setState({ toCurrency }, () => Home.setExchangeRate())
    };

    changeFromValue(event)
    {
        var fromFloat = parseFloat(event.target.value);
        var rateFloat = parseFloat(this.state.rate);
        if (!Number.isNaN(fromFloat) && !Number.isNaN(rateFloat))
        {
            this.setState({ fromValue: (fromFloat * rateFloat) });
        }
    }

    changeToValue(event)
    {
        var toFloat = parseFloat(event.target.value);
        var rateFloat = parseFloat(this.state.rate);
        if (!Number.isNaN(toFloat) && !Number.isNaN(rateFloat))
        {
            this.setState({ fromValue: (toFloat/rateFloat) });
        }
    }

    render()
    {
        let options = this.state.loading ? ['N/A', 'N/A'] : this.state.currencies;
        return (
            <div>
                <input type="text" pattern="[0-9]*" value={this.state.fromValue} onChange={this.changeFromValue} />
                <select onChange={this.changeFrom} value={options[0]}>
                    {options.map(item => (
                        <option key={item} value={item}>
                            {item}
                        </option>
                    ))}
                </select>
                <br />
                <br />
                <input type="text" pattern="[0-9]*" value={this.state.toValue} onChange={this.changeToValue} />
                <select onChange={this.changeTo} value={options[1]}>
                    {options.map(item => (
                        <option key={item} value={item}>
                            {item}
                        </option>
                    ))}
                </select>
            </div>
        );
    }

    async getAvailableCurrencies()
    {
        const response = await fetch('exchangerate/available');
        const data = await response.json();
        this.setState({ currencies: data, loading: false });
    }

    async setExchangeRate()
    {
        const response = await fetch('exchangerate/convert?from=' + this.state.fromCurrency + '&to=' + this.state.toCurrency);
        const data = await response.json();
        this.setState({ rate: data, loading: false });
    }
}
