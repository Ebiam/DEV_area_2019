import React from "react";

import "../../assets/css/Dashboard/Actions.css"

export class ActionReaction extends React.Component {
    constructor(list, ActionReactionCnt, idDnd, optionService, ActionOrReaction) {
        super();
        this.state = {
            currentOption: "",
            curop:""
        };
        this._id = idDnd;
        this._idService = list.id;
        this._name = list.name;
        this._logo = list.logo;
        this._category = ActionOrReaction;
        this._defaultCategory = ActionOrReaction;
        this._optionsList = optionService;
        this.handleDropdownChange = this.handleDropdownChange.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    //  GET
    getId() {return (this._id);}
    getIdService() {return (this._idService);}
    getName() {return (this._name);}
    getLogo() {return (this._logo);}
    getCategory() {return (this._category);}
    getDefaultCategory() {return (this._defaultCategory);}
    getOptionList() {return (this._optionsList);}

    //  SET
    setId(id) {this._id = id;}
    setIdService(idService) {this._idService = idService;}
    setName(name) {this._name = name;}
    setLogo(logo) {this._logo = logo;}
    setCategory(category) {this._category = category;}
    setDefaultCategory(defaultCategory) {this._defaultCategory = defaultCategory;}
    setOptionList(optionList) {this._optionsList = optionList;}

    DisplayOptions() {
        let jsxTab = [];
        jsxTab.push(<option>Select</option>);
        for (let action = 0; this._optionsList[action]; action++)
            jsxTab.push(<option value={this._optionsList[action].id}>{this._optionsList[action].name}</option>);
        return (jsxTab);
    }

    handleDropdownChange(state) {
        if (state.target.id != null)
            this.state.currentOption = state.target.value;
    }

    handleInputChange(state) {
        if (state.target.id != null)
            this.state.curop = state.target.value;
    }

    getCurrentOption()
    {
        return (this.state.currentOption);
    }

    getCurrentOptionInput()
    {
        return (this.state.curop);
    }


    dispBlock() {
        return (<div className="actions">
            <div className="action-block">
                <div>
                    <h5  className="text-uppercase action-text">{this._category}</h5>
                    <span>{this._name}</span>
                </div>
                <div className="div-logo-Action">
                    <img className="logo-action" src={this._logo}/>
                </div>
                <div className="select-option" id="select_option">
                    <select id="dropdown" onChange={this.handleDropdownChange}>
                        {this.DisplayOptions()}
                    </select>
                    <form>
                        <input type="text" name="formtext" onChange={this.handleInputChange}/>
                    </form>
                </div>
            </div>
        </div>);
    }
    render() {
        return (
            <div className="actions">
                <div className="action-block">
                    <div>
                        <h5  className="text-uppercase action-text">{this._category}</h5>
                        <span>{this._name}</span>
                    </div>
                    <div className="div-logo-Action">
                        <img className="logo-action" src={this._logo}/>
                    </div>
                    <div className="select-option" id="select_option">
                            <select id="dropdown" onChange={this.handleDropdownChange}>
                                {this.DisplayOptions()}
                            </select>
                        <form>
                            <input type="text" name="formtext" />
                        </form>
                        <div>Selected value is : {this.state.currentOption}</div>
                    </div>
                </div>
            </div>
        );
    }
}

export default ActionReaction;
